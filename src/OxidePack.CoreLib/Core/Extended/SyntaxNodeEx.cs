using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace OxidePack.CoreLib
{
    public static class SyntaxNodeEx
    {
        public static bool SequenceToRoot(this SyntaxNode one, SyntaxNode two)
        {
            while (true)
            {
                if (one == null && two == null)
                    return true;
                if (one == null || two == null)
                    return false;
                if (one.Kind() != two.Kind())
                    return false;
                one = one.Parent;
                two = two.Parent;
            }
        }
    }
}