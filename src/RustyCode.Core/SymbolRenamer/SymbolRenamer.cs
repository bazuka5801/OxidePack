using System.Text;
using OxidePack.CoreLib.Utils;

namespace OxidePack.CoreLib.Method2Depth.SymbolRenamer
{
    using static SyntaxTreeUtils;

    public class SymbolRenamer
    {
        public string ProcessSource(string source)
        {
            var (compilation, tree) = ParseSource(source);
            var semanticModel = GetSemanticModel(compilation);

            var visitor = new Visitor();
            var visitorResults = visitor.Walk(tree, semanticModel);

            compilation = compilation.RemoveAllSyntaxTrees();

            var sb = new StringBuilder(source);
            foreach (var renameSymbol in visitorResults.RenameSymbols)
            {
                sb.Remove(renameSymbol.StartPosition, renameSymbol.Length);
                sb.Insert(renameSymbol.StartPosition, renameSymbol.Name);
            }

            return sb.ToString();
        }
    }
}