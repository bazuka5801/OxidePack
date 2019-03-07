using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace OxidePack.CoreLib.Experimental.Method2Sequence
{
    public class Method2SequenceReturnFinder : CSharpSyntaxWalker

    {
        private bool _hasReturnKeyword;

        public bool HasReturn(SyntaxNode node)
        {
            Visit(node);

            return _hasReturnKeyword;
        }

        public override void VisitReturnStatement(ReturnStatementSyntax node)
        {
            _hasReturnKeyword = true;
        }
    }
}