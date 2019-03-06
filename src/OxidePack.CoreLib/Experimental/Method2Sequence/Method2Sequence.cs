using System.IO;
using Microsoft.CodeAnalysis.CSharp.Formatting;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis.CSharp.Symbols;
using SapphireEngine.Functions;

namespace OxidePack.CoreLib.Experimental.Method2Sequence
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using static Microsoft.CodeAnalysis.CSharp.SyntaxKind;
    using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
    
    public class Method2Sequence : CSharpSyntaxRewriter
    {
        private static AdhocWorkspace _workspace = new AdhocWorkspace();
        private SemanticModel _semanticModel;
        private ClassDeclarationSyntax _mainClass;
        private ISymbol _mainClassSymbol;
        
        public List<MemberDeclarationSyntax> Members { get; private set; }

        public static IdentifierGenerator IdentifierGenerator = new IdentifierGenerator();


        public string ProcessSource(string source)
        {
            ParseCompilationUnit(source);
            SyntaxTree tree = CSharpSyntaxTree.ParseText(source);
            CompilationUnitSyntax root = tree?.GetCompilationUnitRoot();
            var compilation =  CSharpCompilation.Create("CoreLib")
                .AddReferences(Directory.GetFiles($"references/")
                    .Select(path =>
                        MetadataReference.CreateFromFile(Path.Combine(Directory.GetCurrentDirectory(), path)))
                    .ToList())
                .AddSyntaxTrees(tree);
            var db = new TimeDebugger("Parse", 0.0001f);
                
            _mainClass = FindMainClass(root);
            if (_mainClass == null)
            {
                throw new Exception($"Plugin Class not found in test file");
            }
            

            
            
            _semanticModel = compilation.GetSemanticModel(tree);
            _mainClass = FindMainClass(_semanticModel.SyntaxTree.GetRoot());

            var baseContainer = $"{_mainClass.GetParent<NamespaceDeclarationSyntax>().Name}.{_mainClass.Identifier.Text}";
            var localsVisitor = new Method2SequenceLocalsVisitor();
            var localsResult = localsVisitor.Walk(_mainClass, _semanticModel);
            

            
            var thisVisitor = new Method2SequenceThisVisitor();
            var thisInfo = thisVisitor.Walk(_mainClass, baseContainer, _semanticModel);
            
            var thisRewriter = new Method2SequenceThisRewriter();
            root = (CompilationUnitSyntax)thisRewriter.Rewrite(root, thisInfo);
            
            
            compilation = compilation.RemoveSyntaxTrees(tree).AddSyntaxTrees(tree = tree.WithRootAndOptions(root, CSharpParseOptions.Default));
            root = tree.GetCompilationUnitRoot();
            _semanticModel = compilation.GetSemanticModel(tree);
            
            // After rewrite root
            _mainClass = FindMainClass(_semanticModel.SyntaxTree.GetRoot());
            
            var visitor = new Method2SequenceSyntaxVisitor();
            var results = visitor.Walk(_mainClass);
                
            List<MemberDeclarationSyntax> addedClasses = new List<MemberDeclarationSyntax>();
            
            
            foreach (var method in results.Methods)
            {
                // Key - Method
                // Value - Parent class
                localsResult.GetLocals(method.Value.declaration, method.Value.parentClass, out var locals);
                addedClasses.Add(CreateClassForMethod(method.Value.parentClass, method.Value, locals));
            }

            root = root.ReplaceNode(_mainClass, _mainClass.AddMembers(items: addedClasses.ToArray()));
            
            
            compilation = compilation.RemoveSyntaxTrees(tree).AddSyntaxTrees(tree = tree.WithRootAndOptions(root, CSharpParseOptions.Default));
            root = tree.GetCompilationUnitRoot();
            _semanticModel = compilation.GetSemanticModel(tree);
            
            // After rewrite root
            _mainClass = FindMainClass(_semanticModel.SyntaxTree.GetRoot());
            
            var rewriter = new Method2SequenceRewriter();
            root = (CompilationUnitSyntax)rewriter.Rewrite(root, results);
            
            
            
            compilation = compilation.RemoveAllSyntaxTrees().AddSyntaxTrees(tree = tree.WithRootAndOptions(root, CSharpParseOptions.Default));
            root = tree.GetCompilationUnitRoot();
            _semanticModel = compilation.GetSemanticModel(tree);
            
            // After rewrite root
            _mainClass = FindMainClass(_semanticModel.SyntaxTree.GetRoot());

            var members = _mainClass.Members.ToList();

            if (members.Count > 0 && _mainClass.CloseBraceToken.HasLeadingTrivia)
            {
                var lastElement = members[members.Count - 1];
                members[members.Count - 1] = lastElement.WithTrailingTrivia(lastElement.GetLeadingTrivia()
                    .AddRange(_mainClass.CloseBraceToken.LeadingTrivia));
            }

            this.Members = members.ToList();
            foreach (var data in results.Methods.Values)
            {
                CreatePoolMembers(this.Members, data);
            }
            

            root = root.ReplaceNode(_mainClass, _mainClass.WithMembers(List(this.Members)));
            root = root.NormalizeWhitespace();
            _workspace.Options.WithChangedOption (CSharpFormattingOptions.IndentBraces, true);
            var formattedCode = Formatter.Format (root, _workspace);
            return formattedCode.ToFullString();
        }

        private ClassDeclarationSyntax FindMainClass(SyntaxNode root) => root
            .DescendantNodes(node => node.IsKind(SyntaxKind.ClassDeclaration) == false)
            .OfType<ClassDeclarationSyntax>().FirstOrDefault();

        private void CreatePoolMembers(List<MemberDeclarationSyntax> members, Method2SequenceSyntaxVisitor.Results.ClassData data)
        {
            var listName = IdentifierGenerator.GetNewIdentifier();
            
            members.Add(CreateField(ParseTypeName($"System.Collections.Generic.List<{data.methodClassName}>"), listName, $" new System.Collections.Generic.List<{data.methodClassName}>()")
                .WithModifiers(TokenList(Token(StaticKeyword))));
            members.Add(MethodDeclaration(ParseTypeName("void"), data.pushName)
                .WithParameterList(ParameterList(SeparatedList(new [] {Parameter(ParseToken("p1")).WithType(ParseTypeName(data.methodClassName))})))
                .WithModifiers(TokenList(Token(StaticKeyword)))
                .WithBody(Block(
                    ExpressionStatement(
                        InvocationExpression(
                            MemberAccessExpression(SimpleMemberAccessExpression,
                                IdentifierName(listName), IdentifierName("Add")),
                            ArgumentList(SeparatedList(new []
                            {
                                Argument(IdentifierName("p1"))
                            })))))));
            var parametersZ = data.declaration.ParameterList.Parameters;
            if (parametersZ.All(p=>p.Identifier.ToString() != "_this")&& !data.declaration.Modifiers.Any(p=>p.IsKind(StaticKeyword)))
            parametersZ = parametersZ.Insert(0, Parameter(Identifier("_this")).WithType(ParseTypeName(data.parentClass)));
            var parameters = string.Join(", ",parametersZ
                .Select(p => p.GetFullParameter()).Distinct().ToArray());
            
            members.Add(MethodDeclaration(ParseTypeName(data.methodClassName), data.getName)
                .WithModifiers(TokenList(Token(StaticKeyword)))
                .WithParameterList(ParameterList(parametersZ))
                .WithBody(Block(

                    ParseStatement($"if ({listName}.Count > 0) {{ var temp = {listName}[0];{listName}.RemoveAt(0);return temp;}}"),
                    ReturnStatement(ParseExpression(
                        $"new {data.methodClassName}({parameters})"))
                )));
        }

        private ClassDeclarationSyntax CreateClassForMethod(string parentClass, Method2SequenceSyntaxVisitor.Results.ClassData data, List<(string locName, TypeSyntax locType)> locals)
        {
            var method = data.declaration;
            var constructorParameters =
                SeparatedList(method.ParameterList.Parameters);
            if (constructorParameters.All(p => p.Identifier.ToString() != "_this") && !data.declaration.Modifiers.Any(p=>p.IsKind(StaticKeyword)))
                constructorParameters = constructorParameters.Insert(0,
                    Parameter(Identifier("_this")).WithType(ParseTypeName(parentClass)));
            
            var className = data.methodClassName;
            List<MemberDeclarationSyntax> members = new List<MemberDeclarationSyntax>();
            foreach (var param in constructorParameters)
            {
                 members.Add(CreateField(param.Type, param.Identifier.Text).WithModifiers(TokenList(Token(PublicKeyword))));
            }

            if (locals != null)
            {
                foreach (var localVariable in locals)
                {
                    members.Add(CreateField(localVariable.locType, localVariable.locName).WithModifiers(TokenList(Token(PublicKeyword))));
                }
            }


            members.Add(ConstructorDeclaration(className)
                .WithModifiers(TokenList(Token(PublicKeyword)))
                .WithParameterList(ParameterList(SeparatedList(constructorParameters)))
                .WithBody(Block(
                    constructorParameters.Select(p =>
                        ExpressionStatement(AssignmentExpression(
                        SimpleAssignmentExpression,
                        MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, ThisExpression(),
                            IdentifierName(p.Identifier)),
                        IdentifierName(p.Identifier))))
                )));


            var methodBody = method.Body?.Statements.ToList() ?? new List<StatementSyntax>()
                                 {ExpressionStatement(method.ExpressionBody.Expression)};
            SeparateStatements(method.ReturnType, methodBody, members);
//            methodBody.Insert(0, );
            members.Add(MethodDeclaration(method.ReturnType, method.Identifier.Text+"_execute")
                .WithModifiers(TokenList(Token(PublicKeyword)))
                .WithBody(Block(methodBody)));
            return ClassDeclaration(className).WithMembers(List(members));
        }

        public void SeparateStatements(TypeSyntax returnType, List<StatementSyntax> statements, List<MemberDeclarationSyntax> members)
        {
            IdentifierNameSyntax returnTemp = null;
            for (var i = statements.Count - 1; i >= 0; i--)
            {
                var statement = statements[i];
                if (returnType.ToString() == "void")
                {
                    statement = (StatementSyntax)new Method2SequenceReturnRewriter().Rewrite(statement);
                }
                var methodName = IdentifierGenerator.GetSimpleName();
                
                var retFinder = new Method2SequenceReturnFinder();
                bool hasReturn = retFinder.HasReturn(statement);
                
                var retType = hasReturn ? returnType : ParseTypeName("void");
Console.WriteLine(retType);


                var methodStatements = new List<StatementSyntax>()
                {
                    statement
                };
                if (statement.IsKind(SyntaxKind.ReturnStatement) == false)
                {
                    methodStatements.Add(ReturnStatement(IdentifierName("null")));
                    continue;
                }

                members.Add(MethodDeclaration(ParseTypeName("object"), methodName)
                    .WithModifiers(TokenList(Token(PublicKeyword)))
                    .WithBody(Block(methodStatements)));

                if (hasReturn)
                {
                    if (returnType.ToString() == "void")
                    {
                        statements[i] = IfStatement(ParseExpression($"{methodName}() != null"),
                            ReturnStatement());
                        continue;
                    }
                    if (i == statements.Count - 1)
                    {
                        statements[i] = ( ReturnStatement(CastExpression(returnType,InvocationExpression(IdentifierName(methodName)))));
                        continue;
                    }
                    
                    if (returnTemp == null)
                    {
                        returnTemp = IdentifierName(IdentifierGenerator.GetSimpleName());
                        members.Add(CreateField(ParseTypeName("object"), returnTemp.Identifier.Text)
                            .WithModifiers(TokenList(Token(PublicKeyword))));
                    }

                    statements.RemoveAt(i);
                    statements.InsertRange(i,new StatementSyntax[]
                        {
                            ExpressionStatement(AssignmentExpression(SimpleAssignmentExpression,
                                returnTemp,
                                InvocationExpression(IdentifierName(methodName)))),
                            IfStatement(ParseExpression($"{returnTemp} != null"), ReturnStatement(CastExpression(returnType,returnTemp)))
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