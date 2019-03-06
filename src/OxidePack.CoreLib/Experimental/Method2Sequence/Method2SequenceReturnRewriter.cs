using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
namespace OxidePack.CoreLib.Experimental.Method2Sequence
{
    public class Method2SequenceReturnRewriter : CSharpSyntaxRewriter
    {
        public SyntaxNode Rewrite(SyntaxNode node)
        {
            return Visit(node);
        }

        public override SyntaxNode VisitReturnStatement(ReturnStatementSyntax node)
        {
            if (node.GetParent<MethodDeclarationSyntax>().ReturnType.IsKind(SyntaxKind.VoidKeyword))
            {
                return ReturnStatement(IdentifierName("true"));
            }
            return base.VisitReturnStatement(node);
        }
    }
}