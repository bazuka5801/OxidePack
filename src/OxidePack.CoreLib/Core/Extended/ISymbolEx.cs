using System.Text;
using Microsoft.CodeAnalysis;

namespace OxidePack.CoreLib
{
    public static class ISymbolEx
    {
        public static string FullPath(this ISymbol symbol)
        {
            var sb = new StringBuilder(symbol.Name);
            while ((symbol = symbol.ContainingSymbol) != null)
            {
                sb.Insert(0, $"{symbol.Name}.");
            }

            var result = sb.ToString();
            sb.Clear();
            return result;
        }

        public static bool HasAnyKind(this ISymbol symbol, params SymbolKind[] kinds)
        {
            for (var i = 0; i < kinds.Length; i++)
            {
                if (symbol.Kind == kinds[i])
                {
                    return true;
                }
            }

            return false;
        }

        public static bool IsKind(this ISymbol symbol, SymbolKind kind) => symbol.Kind == kind;
    }
}