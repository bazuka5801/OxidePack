using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace OxidePack.CoreLib.Method2Depth
{
    using static SyntaxFactory;

    public class ReturnRewriter : CSharpSyntaxRewriter
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
                    return ReturnStatement(IdentifierName($"new {_nullStructName}()"));
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
                    return ReturnStatement(IdentifierName($"new {_nullStructName}()"));
                }
            }

            return base.VisitReturnStatement(node);
        }
    }
}