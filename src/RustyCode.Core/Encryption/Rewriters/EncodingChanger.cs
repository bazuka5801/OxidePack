using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using OxidePack.CoreLib.Utils;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace OxidePack.CoreLib
{
    public class EncodingChanger : CSharpSyntaxRewriter
    {
        private readonly AdhocWorkspace _workspace;
        private EncryptorOptions _options;

        public EncodingChanger(AdhocWorkspace workspace, EncryptorOptions options = null,
            bool visitIntoStructuredTrivia = true) : base(visitIntoStructuredTrivia)
        {
            _options = options ?? new EncryptorOptions();
            _workspace = workspace;
        }

        public AdhocWorkspace Process()
        {
            foreach (var project in _workspace.CurrentSolution.Projects)
            {
                foreach (var document in project.Documents.ToList())
                {
                    var node = document.GetSyntaxRootAsync().Result;

                    node = Visit(node);

                    _workspace.TryApplyChanges(document.Project.Solution.WithDocumentSyntaxRoot(document.Id, node));
                }
            }

            return _workspace;
        }

        public override SyntaxNode VisitIdentifierName(IdentifierNameSyntax node) =>
            node.WithIdentifier(Identifier(StringUtils.ConvertToCodePoints(node.Identifier.ToString())));

        public override SyntaxToken VisitToken(SyntaxToken token)
        {
            //Trying to determine if we're visiting a contextual keyword
            if (token.IsKeyword() /*|| token.IsKind(SyntaxKind.InterpolatedStringTextToken)*/)
            {
                var text = StringUtils.ConvertToCodePoints(token.Text);
                return Token(token.LeadingTrivia, token.Kind(), text, text, token.TrailingTrivia);
            }

            if (token.IsKind(SyntaxKind.IdentifierToken))
            {
                var text = StringUtils.ConvertToCodePoints(token.ValueText);
                return Identifier(token.LeadingTrivia, text, token.TrailingTrivia);
            }

            return base.VisitToken(token);
        }

//        public override SyntaxNode VisitLiteralExpression(LiteralExpressionSyntax literal)
//        {
//            if ((literal.IsKind(SyntaxKind.StringLiteralExpression))
//                && string.IsNullOrEmpty(literal.Token.ValueText) == false)
//            {
//                var token = literal.Token;
//                var valueText = StringUtils.ConvertToCodePoints(token.ValueText.ToString());
//                var text = literal.Token.Text.Replace(token.ValueText, valueText);
//                return literal.WithToken(Token(token.LeadingTrivia, token.Kind(), text, valueText, token.TrailingTrivia));
//            }
//            return base.VisitLiteralExpression(literal);
//        }
    }
}