using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace OxidePack.CoreLib.Method2Depth
{
    public class ReturnSearcher : CSharpSyntaxWalker
    {
        private bool _hasReturnKeyword;

        public bool Search(SyntaxNode node)
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