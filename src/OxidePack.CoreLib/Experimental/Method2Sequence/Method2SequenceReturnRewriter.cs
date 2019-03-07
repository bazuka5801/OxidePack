using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace OxidePack.CoreLib.Experimental.Method2Sequence
{
    public class Method2SequenceReturnRewriter : CSharpSyntaxRewriter
    {
        private string _nullStructName;
        private TypeSyntax _returnType;

        public SyntaxNode Rewrite(SyntaxNode node, TypeSyntax returnType, string nullStructName)
        {
            _returnType = returnType;
            _nullStructName = nullStructName;
            return Visit(node);
        }

        public override SyntaxNode VisitReturnStatement(ReturnStatementSyntax node)
        {
            if (_returnType.ToString() != "void")
            {
                var method = node.GetParent<MethodDeclarationSyntax>();
                if (method.ReturnType.IsKind(SyntaxKind.VoidKeyword))
                {
                    return ReturnStatement(IdentifierName("true"));
                }

                if (node.Expression?.ToString() == "null")
                {
                    return ReturnStatement(ParseExpression($"new {_nullStructName}()"));
                }
            }
            else
            {
                if (node.Expression == null)
                {
                    return ReturnStatement(IdentifierName("true"));
                }
            }

            return base.VisitReturnStatement(node);
        }
    }
}