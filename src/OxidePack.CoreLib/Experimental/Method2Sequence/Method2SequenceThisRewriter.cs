using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace OxidePack.CoreLib.Experimental.Method2Sequence.Old
{
    public class Method2SequenceThisRewriter : CSharpSyntaxRewriter
    {
        private Method2SequenceThisVisitor.Results _thisInfo;

        public SyntaxNode Rewrite(SyntaxNode root, Method2SequenceThisVisitor.Results thisInfo)
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
            var method = node.GetParent<MethodDeclarationSyntax>();
            if (method != null && _thisInfo.IdentifiersNeedsThis.Contains(node))
            {
                return node.WithIdentifier(Identifier(
                    $"{_thisInfo.ThisNames[method.FullPath()]}." +
                    node.Identifier.Text));
            }

            return base.VisitIdentifierName(node);
        }

        public override SyntaxNode VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            if (node.ExpressionBody != null)
            {
                node = (MethodDeclarationSyntax) base.VisitMethodDeclaration(node);
                var body = node.ReturnType.ToString() == "void"
                        ? ExpressionStatement(node.ExpressionBody.Expression)
                        : (StatementSyntax)ReturnStatement(node.ExpressionBody.Expression);
                return MethodDeclaration(node.AttributeLists,
                    node.Modifiers,
                    node.ReturnType,
                    node.ExplicitInterfaceSpecifier,
                    node.Identifier,
                    node.TypeParameterList,
                    node.ParameterList,
                    node.ConstraintClauses,
                    Block(body), ParseToken(""));
            }

            return base.VisitMethodDeclaration(node);
        }
    }
}