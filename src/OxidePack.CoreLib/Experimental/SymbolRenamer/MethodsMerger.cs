using System;
using System.Diagnostics;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Rename;
using Microsoft.CodeAnalysis.Text;

namespace OxidePack.CoreLib.Experimental.SymbolRenamer
{
    using static Utils.SyntaxTreeUtils;
    
    public class SymbolRenamer
    {
        public string ProcessSource(string source)
        {
            var (compilation, tree) = ParseSource(source);
            var semanticModel = GetSemanticModel(compilation);
            
            var visitor = new Visitor();
            var visitorResults = visitor.Walk(tree, semanticModel);
            
            compilation = compilation.RemoveAllSyntaxTrees();
            
            StringBuilder sb = new StringBuilder(source);
            foreach (var renameSymbol in visitorResults.RenameSymbols)
            {
                sb.Remove(renameSymbol.StartPosition, renameSymbol.Length);
                sb.Insert(renameSymbol.StartPosition, renameSymbol.Name);
            }

            return sb.ToString();
        }
        
        
        
    }
}