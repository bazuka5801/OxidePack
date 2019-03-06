using System;
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
            _thisInfo.IdentifiersNeedsThis.ForEach(Console.WriteLine);
            Console.WriteLine();
            return Visit(root);
        }

        public override SyntaxNode VisitThisExpression(ThisExpressionSyntax node)
        {
            if (_thisInfo.ThisExpressions.Contains(node))
            {
                return IdentifierName("_this");
            }

            return base.VisitThisExpression(node);
        }

        public override SyntaxNode VisitMemberAccessExpression(MemberAccessExpressionSyntax node)
        {
            if (node.Expression.IsKind(SyntaxKind.ThisExpression) && node.GetParent<ConstructorDeclarationSyntax>() == null)
            {
                //return node.Name.WithIdentifier(Identifier("_this." + node.Name.Identifier.Text));
            }

            return base.VisitMemberAccessExpression(node);
        }

        public override SyntaxNode VisitIdentifierName(IdentifierNameSyntax node)
        {
            if (node.GetParent<MethodDeclarationSyntax>()?.Identifier.Text == "Unload_execute")
            {
                
            }
            if (_thisInfo.IdentifiersNeedsThis.Contains(node))
            {
                return node.WithIdentifier(Identifier("_this." + node.Identifier.Text));
            }
//            else if (node.Parent is MemberAccessExpressionSyntax ms && ms.Expression.IsKind(SyntaxKind.ThisExpression))
//            {
//                
//            }
            return base.VisitIdentifierName(node);
        }

        public override SyntaxNode VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            if (node.ExpressionBody != null)
            {
                node = (MethodDeclarationSyntax)base.VisitMethodDeclaration(node);
                var body = ExpressionStatement(node.ExpressionBody.Expression);
                return MethodDeclaration(node.AttributeLists, 
                    node.Modifiers, 
                    node.ReturnType, 
                    node.ExplicitInterfaceSpecifier,
                    node.Identifier,
                    node.TypeParameterList,
                    node.ParameterList,
                    node.ConstraintClauses,
                    Block(body), ParseToken(""));
//                    .WithExpressionBody(default)
//                    .WithBody(Block(body))
//                    .WithTrailingTrivia(null);
            }
            return base.VisitMethodDeclaration(node);
        }
    }
}