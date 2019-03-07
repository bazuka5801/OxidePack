using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace OxidePack.CoreLib.Experimental.ControlFlowObfuscation
{
    public class ControlFlowRewriter : CSharpSyntaxRewriter
    {
        public SyntaxNode Rewrite(SyntaxNode node)
        {
            return Visit(node);
        }

        public override SyntaxNode VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            node = (ClassDeclarationSyntax) base.VisitClassDeclaration(node);

            var members = node.Members.ToList();
            var newMembers = new List<MemberDeclarationSyntax>();
            for (var i = members.Count - 1; i >= 0; i--)
            {
                if (members[i] is MethodDeclarationSyntax method)
                {
                    StatementSyntax endStatement = null;
                    if (method.ReturnType.ToString() != "void")
                        endStatement = ThrowStatement(ParseExpression("new System.Exception()"));
                    var statements = method.Body.Statements.ToList();
                    ControlFlowGenerator.Generate(statements, newMembers, endStatement);
                    node = node.ReplaceNode(method, method.WithBody(Block(statements)));
                }
            }

            return node.AddMembers(newMembers.ToArray());
        }
    }
}