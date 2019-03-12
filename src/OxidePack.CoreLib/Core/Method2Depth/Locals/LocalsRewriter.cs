using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace OxidePack.CoreLib.Method2Depth
{
    using static SyntaxFactory;
    using static SyntaxKind;

    public class LocalsRewriter : CSharpSyntaxRewriter
    {
        private MethodsVisitorResults _methodsVisitorResults;

        public SyntaxNode Rewrite(SyntaxNode node, MethodsVisitorResults methodsVisitorResults)
        {
            _methodsVisitorResults = methodsVisitorResults;

            node = Visit(node);

            return node;
        }

        public override SyntaxNode VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            if (_methodsVisitorResults.Methods.ContainsKey(node.FullPath()) == false)
            {
                return node;
            }

            return base.VisitMethodDeclaration(node);
        }

        public override SyntaxNode VisitBlock(BlockSyntax node)
        {
            node = (BlockSyntax) base.VisitBlock(node);
            var statements = node.Statements.ToList();
            var flag = false;
            for (var i = statements.Count - 1; i >= 0; i--)
            {
                var statement = statements[i];
                if (statement is LocalDeclarationStatementSyntax local)
                {
                    statements.RemoveAt(i);
                    statements.InsertRange(i, LocalToAssignments(local));
                    flag = true;
                }
            }

            if (flag)
            {
                return node.WithStatements(List(statements));
            }

            return node;
        }

        /// <summary>
        ///     int a = 2, b; => a = 2;
        /// </summary>
        /// <param name="node">Local Declaration Statement</param>
        /// <returns>Assignment Statements</returns>
        private ExpressionStatementSyntax[] LocalToAssignments(LocalDeclarationStatementSyntax node)
        {
            return node.Declaration.Variables
                .Where(p => p.Initializer != null)
                .Select(p =>
                    ExpressionStatement(AssignmentExpression(
                        SimpleAssignmentExpression,
                        IdentifierName(p.Identifier),
                        p.Initializer.Value))).ToArray();
        }
    }
}