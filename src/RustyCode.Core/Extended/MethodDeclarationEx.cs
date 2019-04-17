using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace OxidePack.CoreLib
{
    public static class MethodDeclarationEx
    {
        public static bool HasModifier(this MethodDeclarationSyntax method, SyntaxKind modifier)
        {
            return method.Modifiers.Any(p => p.IsKind(modifier));
        }
    }
}