using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Formatting;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using SapphireEngine.Functions;

namespace OxidePack.CoreLib.Experimental.Method2Sequence
{
    using static SyntaxKind;
    using static SyntaxFactory;

    public class Method2Sequence : CSharpSyntaxRewriter
    {
        private static readonly AdhocWorkspace Workspace = new AdhocWorkspace();
        private ClassDeclarationSyntax _mainClass;
        private ISymbol _mainClassSymbol;
        private string _nullStructName;
        private SemanticModel _semanticModel;
        private Method2SequenceThisVisitor.Results _thisInfo;

        public string ProcessSource(string source)
        {
            ParseCompilationUnit(source);
            var tree = CSharpSyntaxTree.ParseText(source);
            var root = tree?.GetCompilationUnitRoot();
            var compilation = CSharpCompilation.Create("CoreLib")
                .AddReferences(Directory.GetFiles("references/")
                    .Select(path =>
                        MetadataReference.CreateFromFile(Path.Combine(Directory.GetCurrentDirectory(), path)))
                    .ToList())
                .AddSyntaxTrees(tree);
            var db = new TimeDebugger("Parse", 0.0001f);

            _mainClass = FindMainClass(root);


            _semanticModel = compilation.GetSemanticModel(tree);
            _mainClass = FindMainClass(_semanticModel.SyntaxTree.GetRoot());

            var baseContainer =
                $"{_mainClass.GetParent<NamespaceDeclarationSyntax>().Name}.{_mainClass.Identifier.Text}";
            var localsVisitor = new Method2SequenceLocalsVisitor();
            var localsResult = localsVisitor.Walk(_mainClass, _semanticModel);


            var thisVisitor = new Method2SequenceThisVisitor();
            _thisInfo = thisVisitor.Walk(_mainClass, baseContainer, _semanticModel);

            var thisRewriter = new Method2SequenceThisRewriter();
            root = (CompilationUnitSyntax) thisRewriter.Rewrite(root, _thisInfo);

            var nullStruct = CreateNullReturnValue();
            _nullStructName = nullStruct.Identifier.Text;


            compilation = compilation.RemoveSyntaxTrees(tree)
                .AddSyntaxTrees(tree = tree.WithRootAndOptions(root, CSharpParseOptions.Default));
            root = tree.GetCompilationUnitRoot();
            _semanticModel = compilation.GetSemanticModel(tree);

            // After rewrite root
            _mainClass = FindMainClass(_semanticModel.SyntaxTree.GetRoot());

            var visitor = new Method2SequenceSyntaxVisitor();
            var results = visitor.Walk(_mainClass);

            var addedClasses = new List<MemberDeclarationSyntax>();


            foreach (var method in results.Methods)
            {
                // Key - Method
                // Value - Parent class
                localsResult.GetLocals(method.Value.declaration, method.Value.parentClass, out var locals);

                var mClass = CreateClassForMethod(method.Value.parentClass,
                    _thisInfo.ThisNames[method.Value.declaration.FullPath()], method.Value, locals);
//                mClass = (ClassDeclarationSyntax) new ControlFlowRewriter().Rewrite(mClass);
                addedClasses.Add(mClass);
            }

            addedClasses.Add(nullStruct);

            root = root.ReplaceNode(_mainClass, _mainClass.AddMembers(addedClasses.ToArray()));


            compilation = compilation.RemoveSyntaxTrees(tree)
                .AddSyntaxTrees(tree = tree.WithRootAndOptions(root, CSharpParseOptions.Default));
            root = tree.GetCompilationUnitRoot();
            _semanticModel = compilation.GetSemanticModel(tree);

            // After rewrite root
            _mainClass = FindMainClass(_semanticModel.SyntaxTree.GetRoot());

            var rewriter = new Method2SequenceRewriter();
            root = (CompilationUnitSyntax) rewriter.Rewrite(root, results);


            compilation = compilation.RemoveAllSyntaxTrees()
                .AddSyntaxTrees(tree = tree.WithRootAndOptions(root, CSharpParseOptions.Default));
            root = tree.GetCompilationUnitRoot();
            _semanticModel = compilation.GetSemanticModel(tree);

            // After rewrite root
            _mainClass = FindMainClass(_semanticModel.SyntaxTree.GetRoot());

            var members = _mainClass.Members.ToList();
            foreach (var data in results.Methods.Values)
            {
                CreatePoolMembers(members, data);
            }

            root = root.ReplaceNode(_mainClass, _mainClass.WithMembers(List(members)));

            root = root.NormalizeWhitespace();
            Workspace.Options.WithChangedOption(CSharpFormattingOptions.IndentBraces, true);
            var formattedCode = Formatter.Format(root, Workspace);
            return formattedCode.ToFullString();
        }

        private ClassDeclarationSyntax FindMainClass(SyntaxNode root) => root
            .DescendantNodes(node => node.IsKind(SyntaxKind.ClassDeclaration) == false)
            .OfType<ClassDeclarationSyntax>().FirstOrDefault();

        private StructDeclarationSyntax CreateNullReturnValue() =>
            StructDeclaration(IdentifierGenerator.GetSimpleName())
                .WithModifiers(TokenList(Token(PublicKeyword)));

        private void CreatePoolMembers(List<MemberDeclarationSyntax> members,
            Method2SequenceSyntaxVisitor.Results.ClassData data)
        {
            var listName = IdentifierGenerator.GetSimpleName();

            members.Add(CreateField(ParseTypeName($"System.Collections.Generic.List<{data.methodClassName}>"), listName,
                    $" new System.Collections.Generic.List<{data.methodClassName}>()")
                .WithModifiers(TokenList(Token(StaticKeyword))));
            var pName = IdentifierGenerator.GetSimpleName();
            members.Add(MethodDeclaration(ParseTypeName("void"), data.pushName)
                .WithParameterList(ParameterList(SeparatedList(new[]
                    {Parameter(ParseToken(pName)).WithType(ParseTypeName(data.methodClassName))})))
                .WithModifiers(TokenList(Token(StaticKeyword)))
                .WithBody(Block(
                    ExpressionStatement(
                        InvocationExpression(
                            MemberAccessExpression(SimpleMemberAccessExpression,
                                IdentifierName(listName), IdentifierName("Add")),
                            ArgumentList(SeparatedList(new[]
                            {
                                Argument(IdentifierName(pName))
                            })))))));
            var parametersZ = data.declaration.ParameterList.Parameters;
            if (!data.declaration.Modifiers.Any(p => p.IsKind(StaticKeyword)))
            {
                var thisName = _thisInfo.ThisNames[data.declaration.FullPath()];
                parametersZ = parametersZ.Insert(0,
                    Parameter(Identifier(thisName)).WithType(ParseTypeName(data.parentClass)));
            }

            var parameters = string.Join(", ", parametersZ
                .Select(p => p.GetFullParameter()).Distinct().ToArray());
            var tempName = IdentifierGenerator.GetSimpleName();
            members.Add(MethodDeclaration(ParseTypeName(data.methodClassName), data.getName)
                .WithModifiers(TokenList(Token(StaticKeyword)))
                .WithParameterList(ParameterList(parametersZ))
                .WithBody(Block(
                    ExpressionStatement(ParseExpression($"{data.methodClassName} {tempName}")),
                    IfStatement(ParseExpression($"{listName}.Count > 0"), Block(
                        ExpressionStatement(AssignmentExpression(SimpleAssignmentExpression,
                            IdentifierName($"{tempName}"), ParseExpression($"{listName}[0]"))),
                        ParseStatement($"{listName}.RemoveAt(0);"),
                        ParseStatement($"{tempName}.{data.methodClassInitializeMethodName}({parameters});"),
                        ReturnStatement(IdentifierName($"{tempName}"))
                    )),
                    ExpressionStatement(AssignmentExpression(SimpleAssignmentExpression,
                        IdentifierName($"{tempName}"), ParseExpression($"new {data.methodClassName}()"))),
                    ParseStatement($"{tempName}.{data.methodClassInitializeMethodName}({parameters});"),
                    ReturnStatement(IdentifierName($"{tempName}"))
                )));
        }

        private ClassDeclarationSyntax CreateClassForMethod(string parentClass, string thisKeyword,
            Method2SequenceSyntaxVisitor.Results.ClassData data, List<(string locName, TypeSyntax locType)> locals)
        {
            var method = data.declaration;
            var initializingParameters =
                SeparatedList(method.ParameterList.Parameters);
            if (!data.declaration.Modifiers.Any(p => p.IsKind(StaticKeyword)))
            {
                initializingParameters = initializingParameters.Insert(0,
                    Parameter(Identifier(thisKeyword)).WithType(ParseTypeName(parentClass)));
            }

            var className = data.methodClassName;
            var members = new List<MemberDeclarationSyntax>();
            foreach (var param in initializingParameters)
            {
                members.Add(CreateField(param.Type, param.Identifier.Text)
                    .WithModifiers(TokenList(Token(PublicKeyword))));
            }

            if (locals != null)
            {
                foreach (var localVariable in locals)
                {
                    members.Add(CreateField(localVariable.locType, localVariable.locName)
                        .WithModifiers(TokenList(Token(PublicKeyword))));
                }
            }

            members.Add(MethodDeclaration(ParseTypeName("void"), data.methodClassInitializeMethodName)
                .WithModifiers(TokenList(Token(PublicKeyword)))
                .WithParameterList(ParameterList(SeparatedList(initializingParameters)))
                .WithBody(Block(
                    initializingParameters.Select(p =>
                        ExpressionStatement(AssignmentExpression(
                            SimpleAssignmentExpression,
                            MemberAccessExpression(SimpleMemberAccessExpression, ThisExpression(),
                                IdentifierName(p.Identifier)),
                            IdentifierName(p.Identifier))))
                )));

            var methodBody = method.Body?.Statements.ToList() ?? new List<StatementSyntax>
                                 {ExpressionStatement(method.ExpressionBody.Expression)};
            SeparateStatements(method.ReturnType, methodBody, members);

            members.Add(MethodDeclaration(method.ReturnType, data.methodClassMethodName)
                .WithModifiers(TokenList(Token(PublicKeyword)))
                .WithBody(Block(methodBody)));
            return ClassDeclaration(className).WithMembers(List(members));
        }

        public void SeparateStatements(TypeSyntax returnType, List<StatementSyntax> statements,
            List<MemberDeclarationSyntax> members)
        {
            IdentifierNameSyntax returnTemp = null;
            for (var i = statements.Count - 1; i >= 0; i--)
            {
                var statement = statements[i];
                statement = (StatementSyntax) new Method2SequenceReturnRewriter().Rewrite(statement, returnType,
                    _nullStructName);
                var methodName = IdentifierGenerator.GetSimpleName();

                var retFinder = new Method2SequenceReturnFinder();
                var hasReturn = retFinder.HasReturn(statement);

                var methodStatements = new List<StatementSyntax>
                {
                    statement
                };

                if (statement.IsKind(SyntaxKind.ReturnStatement) == false)
                {
                    methodStatements.Add(ReturnStatement(IdentifierName("null")));
                }

                members.Add(MethodDeclaration(ParseTypeName("object"), methodName)
                    .WithModifiers(TokenList(Token(PublicKeyword)))
                    .WithBody(Block(methodStatements)));

                if (hasReturn)
                {
                    if (returnType.ToString() == "void")
                    {
                        statements[i] = IfStatement(ParseExpression($"{methodName}() is {_nullStructName}"),
                            ReturnStatement());
                        continue;
                    }

                    if (returnTemp == null)
                    {
                        returnTemp = IdentifierName(IdentifierGenerator.GetSimpleName());
                        members.Add(CreateField(ParseTypeName("object"), returnTemp.Identifier.Text)
                            .WithModifiers(TokenList(Token(PublicKeyword))));
                    }

                    if (i == statements.Count - 1)
                    {
                        statements.RemoveAt(i);
                        statements.InsertRange(i, new StatementSyntax[]
                        {
                            ExpressionStatement(AssignmentExpression(SimpleAssignmentExpression,
                                returnTemp,
                                InvocationExpression(IdentifierName(methodName)))),
                            IfStatement(ParseExpression($"{returnTemp} is {_nullStructName}"),
                                ReturnStatement(IdentifierName($"default({returnType.ToString()})"))),
                            returnType.ToString() == "object"
                                ? ReturnStatement(returnTemp)
                                : ReturnStatement(CastExpression(returnType, returnTemp))
                        });
                        continue;
                    }

                    statements.RemoveAt(i);
                    statements.InsertRange(i, new StatementSyntax[]
                    {
                        ExpressionStatement(AssignmentExpression(SimpleAssignmentExpression,
                            returnTemp,
                            InvocationExpression(IdentifierName(methodName)))),
                        IfStatement(ParseExpression($"{returnTemp} is {_nullStructName}"),
                            ReturnStatement(IdentifierName($"default({returnType.ToString()})"))),
                        IfStatement(ParseExpression($"{returnTemp} != null"),
                            ReturnStatement(returnType.ToString() == "object"
                                ? (ExpressionSyntax) returnTemp
                                : CastExpression(returnType, returnTemp)))
                    });
                }
                else
                {
                    statements[i] = ExpressionStatement(InvocationExpression(IdentifierName(methodName)));
                }
            }
        }

        private FieldDeclarationSyntax CreateField(TypeSyntax type, string identifier) =>
            FieldDeclaration(default, SyntaxTokenList.Create(Token(PrivateKeyword)),
                VariableDeclaration(type, SeparatedList(new[] {VariableDeclarator(identifier)})),
                Token(SemicolonToken));

        private FieldDeclarationSyntax CreateField(TypeSyntax type, string identifier, string initializer) =>
            FieldDeclaration(default, SyntaxTokenList.Create(Token(PrivateKeyword)),
                VariableDeclaration(type, SeparatedList(new[]
                {
                    VariableDeclarator(identifier)
                        .WithInitializer(
                            EqualsValueClause(ParseExpression(initializer)))
                })),
                Token(SemicolonToken));
    }
}