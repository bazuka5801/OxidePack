using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
namespace OxidePack.CoreLib.Experimental.Method2Sequence
{
    public class Method2SequenceReturnRewriter : CSharpSyntaxRewriter
    {
        private TypeSyntax _returnType;
        
        public SyntaxNode Rewrite(SyntaxNode node, TypeSyntax returnType)
        {
            _returnType = returnType;
            return Visit(node);
        }

        public override SyntaxNode VisitReturnStatement(ReturnStatementSyntax node)
        {
            if (_returnType.ToString() != "void")
            {
                if (node.GetParent<MethodDeclarationSyntax>().ReturnType.IsKind(SyntaxKind.VoidKeyword))
                {
                    return ReturnStatement(IdentifierName("true"));
                }
            }
            else
            {
                if (node.Expression == null)
                {
                    return ReturnStatement(IdentifierName("null"));
                }
            }

            return base.VisitReturnStatement(node);
        }
    }
}