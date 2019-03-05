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
        
        private List<MemberDeclarationSyntax> _addedMembers = new List<MemberDeclarationSyntax>();
        public List<MemberDeclarationSyntax> Members { get; private set; }



        public string ProcessSource(string source)
        {
            ParseCompilationUnit(source);
            SyntaxTree tree = CSharpSyntaxTree.ParseText(source);
            CompilationUnitSyntax root = tree?.GetCompilationUnitRoot();
            var compilation =  CSharpCompilation.Create("HelloWorld")
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
            
            

            var thisVisitor = new Method2SequenceThisVisitor();
            var thisInfo = thisVisitor.Walk(_mainClass, _semanticModel);
            var thisRewriter = new Method2SequenceThisRewriter();
            root = (CompilationUnitSyntax)thisRewriter.Rewrite(root, thisInfo);

            // After rewrite root
            _mainClass = FindMainClass(root);
            
            var visitor = new Method2SequenceVisitor();
            var results = visitor.Walk(_mainClass);
                
            List<MemberDeclarationSyntax> addedClasses = new List<MemberDeclarationSyntax>();
            
            
            foreach (var method in results.Methods)
            {
                // Key - Method
                // Value - Parent class
                results.GetLocals(method.Value.declaration, method.Value.parentClass, out var locals);
                addedClasses.Add(CreateClassForMethod(method.Value.parentClass, method.Value.declaration, locals));
            }

            root = root.ReplaceNode(_mainClass, _mainClass.AddMembers(items: addedClasses.ToArray()));
            
            
            compilation = compilation.RemoveSyntaxTrees(tree).AddSyntaxTrees(tree = tree.WithRootAndOptions(root, CSharpParseOptions.Default));
            root = tree.GetCompilationUnitRoot();
            _semanticModel = compilation.GetSemanticModel(tree);

            // After rewrite tree
            _mainClass = FindMainClass(tree.GetRoot());
            
            var rewriter = new Method2SequenceRewriter();
            root = (CompilationUnitSyntax)rewriter.Rewrite(root, results);
            
            // After rewrite tree
            _mainClass = FindMainClass(tree.GetRoot());

            var members = _mainClass.Members.ToList();

            if (members.Count > 0 && _mainClass.CloseBraceToken.HasLeadingTrivia)
            {
                var lastElement = members[members.Count - 1];
                members[members.Count - 1] = lastElement.WithTrailingTrivia(lastElement.GetLeadingTrivia()
                    .AddRange(_mainClass.CloseBraceToken.LeadingTrivia));
            }

            this.Members = members.ToList();
            

            root = root.ReplaceNode(_mainClass, _mainClass.WithMembers(List(this.Members)));
            root = root.NormalizeWhitespace();
            _workspace.Options.WithChangedOption (CSharpFormattingOptions.IndentBraces, true);
            var formattedCode = Formatter.Format (root, _workspace);
            return formattedCode.ToFullString();
        }

        private ClassDeclarationSyntax FindMainClass(SyntaxNode root) => root
            .DescendantNodes(node => node.IsKind(SyntaxKind.ClassDeclaration) == false)
            .OfType<ClassDeclarationSyntax>().FirstOrDefault();

        private ClassDeclarationSyntax CreateClassForMethod(string parentClass, MethodDeclarationSyntax method, List<(string locName, TypeSyntax locType)> locals)
        {
            var constructorParameters =
                SeparatedList(method.ParameterList.Parameters).Insert(0, Parameter(Identifier("_this")).WithType(ParseTypeName(parentClass)));
            
            
            
            var className = $"{parentClass}_{method.Identifier.Text}_sequence";
            List<MemberDeclarationSyntax> members = new List<MemberDeclarationSyntax>();
            foreach (var param in constructorParameters)
            {
                 members.Add(CreateField(param.Type, param.Identifier.Text));
            }

            if (locals != null)
            {
                foreach (var localVariable in locals)
                {
                    members.Add(CreateField(localVariable.locType, localVariable.locName));
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
            
            members.Add(MethodDeclaration(method.ReturnType, method.Identifier.Text+"_execute")
                .WithBody(method.Body));
            return ClassDeclaration(className).WithMembers(List(members));
        }

        private FieldDeclarationSyntax CreateField(TypeSyntax type, string identifier) =>
            FieldDeclaration(default, SyntaxTokenList.Create(Token(PrivateKeyword)),
                VariableDeclaration(type, SeparatedList(new[] {VariableDeclarator(identifier)})),
                Token(SemicolonToken));
    }
}