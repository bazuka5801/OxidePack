using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace OxidePack.CoreLib.Method2Depth
{
    using static SyntaxFactory;

    public class ThisRewriter : CSharpSyntaxRewriter
    {
        private ThisVisitorResults _thisInfo;

        public SyntaxNode Rewrite(SyntaxNode root, ThisVisitorResults thisInfo)
        {
            _thisInfo = thisInfo;
            return Visit(root);
        }

        public override SyntaxNode VisitThisExpression(ThisExpressionSyntax node)
        {
            if (_thisInfo.ThisExpressions.Contains(node))
            {
                return IdentifierName(_thisInfo.ThisNames[node.GetParent<MethodDeclarationSyntax>().FullPath()]);
            }

            return base.VisitThisExpression(node);
        }

        public override SyntaxNode VisitIdentifierName(IdentifierNameSyntax node)
        {
            if (node.ToString() == "Spawner__OnEntityKill")
            {
            }

            var method = node.GetParent<MethodDeclarationSyntax>();
            if (method != null && _thisInfo.IdentifiersNeedsThis.Contains(node.Identifier))
            {
                return node.WithIdentifier(
                    Identifier($"{_thisInfo.ThisNames[method.FullPath()]}.{node.Identifier.Text}"));
            }

            return base.VisitIdentifierName(node);
        }

        public override SyntaxNode VisitGenericName(GenericNameSyntax node)
        {
            var method = node.GetParent<MethodDeclarationSyntax>();
            if (method != null && _thisInfo.IdentifiersNeedsThis.Contains(node.Identifier))
            {
                return node.WithIdentifier(
                    Identifier($"{_thisInfo.ThisNames[method.FullPath()]}.{node.Identifier.Text}"));
            }

            return base.VisitGenericName(node);
        }

        public override SyntaxNode VisitBaseExpression(BaseExpressionSyntax node)
        {
            var method = node.GetParent<MethodDeclarationSyntax>();
            if (method != null && _thisInfo.IdentifiersNeedsThis.Contains(node.Token))
            {
                return IdentifierName($"{_thisInfo.ThisNames[method.FullPath()]}");
            }

            return base.VisitBaseExpression(node);
        }

        public override SyntaxNode VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            node = (MethodDeclarationSyntax) base.VisitMethodDeclaration(node);
            if (node.ExpressionBody != null)
            {
                node = (MethodDeclarationSyntax) base.VisitMethodDeclaration(node);
                var body = node.ReturnType.ToString() == "void"
                    ? ExpressionStatement(node.ExpressionBody.Expression)
                    : (StatementSyntax) ReturnStatement(node.ExpressionBody.Expression);
                var method = MethodDeclaration(node.AttributeLists,
                    node.Modifiers,
                    node.ReturnType,
                    node.ExplicitInterfaceSpecifier,
                    node.Identifier,
                    node.TypeParameterList,
                    node.ParameterList,
                    node.ConstraintClauses,
                    Block(body), ParseToken(""));

                return method;
            }

            return node;
        }
    }
}