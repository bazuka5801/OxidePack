using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Formatting;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;

namespace OxidePack.CoreLib.Utils
{
    using static SyntaxKind;
    using static SyntaxFactory;
    
    public static class SyntaxTreeUtils
    {
        private static readonly AdhocWorkspace Workspace = new AdhocWorkspace();

        public static (CSharpCompilation compilation, CompilationUnitSyntax root) ParseSource(string source, string referencesPath = "references/")
        {
            var tree = CSharpSyntaxTree.ParseText(source);
            var root = tree?.GetCompilationUnitRoot();
            var compilation = CSharpCompilation.Create("CoreLib")
                .AddReferences(Directory.GetFiles(referencesPath)
                    .Select(path =>
                        MetadataReference.CreateFromFile(Path.Combine(Directory.GetCurrentDirectory(), path)))
                    .ToList())
                .AddSyntaxTrees(tree);
            
            return (compilation, root);
        }

        public static SemanticModel GetSemanticModel(CSharpCompilation compilation, int syntaxTreeIndex = 0)
        {
            return compilation.GetSemanticModel(compilation.SyntaxTrees[syntaxTreeIndex]);
        }

        public static string ConvertToSourceText(CompilationUnitSyntax root, bool normalizeWhitespace = true)
        {
            if (normalizeWhitespace)
                root = root.NormalizeWhitespace();
            Workspace.Options.WithChangedOption(CSharpFormattingOptions.IndentBraces, true);
            var formattedCode = Formatter.Format(root, Workspace);
            return formattedCode.ToFullString();
        }
    }
}