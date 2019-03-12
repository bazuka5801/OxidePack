using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace OxidePack.CoreLib.Method2Depth
{
    using static SyntaxFactory;
    using static SyntaxKind;

    internal partial class Core
    {
        private StructDeclarationSyntax CreateNullReturnValue() =>
            StructDeclaration(IdentifierGenerator.GetSimpleName())
                .WithModifiers(TokenList(Token(PublicKeyword)));
    }
}