using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Microsoft.CodeAnalysis.CSharp.SyntaxKind;

namespace OxidePack.CoreLib.Experimental.Method2Sequence
{
    public class Method2SequenceThisRewriter : CSharpSyntaxRewriter
    {
        
        private Method2SequenceThisVisitor.Results _thisInfo;
        
        public SyntaxNode Rewrite(SyntaxNode root, Method2SequenceThisVisitor.Results thisInfo)
        {
            _thisInfo = thisInfo;
            return Visit(root);
        }
        

        public override SyntaxNode VisitIdentifierName(IdentifierNameSyntax node)
        {
            if (_thisInfo.IdentifiersNeedsThis.Contains(node))
            {
                return node.WithIdentifier(Identifier("_this." + node.Identifier.Text));
            }

            return base.VisitIdentifierName(node);
        }
    }
}