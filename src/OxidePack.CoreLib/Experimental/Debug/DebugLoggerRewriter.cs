using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Formatting;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
namespace OxidePack.CoreLib.Experimental.Debug
{
    public class DebugLoggerRewriter : CSharpSyntaxRewriter
    {
        private static AdhocWorkspace _workspace = new AdhocWorkspace();
        
        private int i = 0;
        
        public static string Process(string source)
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
            
            root = (CompilationUnitSyntax)new DebugLoggerRewriter().Visit(root);
            
            root = root.NormalizeWhitespace();
            _workspace.Options.WithChangedOption (CSharpFormattingOptions.IndentBraces, true);
            var formattedCode = Formatter.Format (root, _workspace);
            return formattedCode.ToFullString();
            
        }
        
        public override SyntaxNode VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            node = (MethodDeclarationSyntax) base.VisitMethodDeclaration(node);
            var statements = node.Body.Statements.ToList();
            statements.Insert(0,SyntaxFactory.ParseStatement($"global::Oxide.Core.Interface.Oxide.LogInfo(\"{node.Identifier.Text}\");"));
            return (node).WithBody(node.Body.WithStatements(SyntaxFactory.List(statements)));
        }
    }
}