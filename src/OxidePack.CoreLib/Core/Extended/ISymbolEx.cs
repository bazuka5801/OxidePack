using System.Text;
using Microsoft.CodeAnalysis;

namespace OxidePack.CoreLib
{
    public static class ISymbolEx
    {
        public static string FullPath(this ISymbol symbol)
        {
            StringBuilder sb = new StringBuilder(symbol.Name);
            while ((symbol = symbol.ContainingSymbol) != null)
            {
                sb.Insert(0, $"{symbol.Name}.");
            }

            string result = sb.ToString();
            sb.Clear();
            return result;
        }
        
    }
}