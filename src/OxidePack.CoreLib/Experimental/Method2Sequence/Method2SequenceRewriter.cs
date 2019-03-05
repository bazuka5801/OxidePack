using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Formatting;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis.CSharp.Symbols;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Microsoft.CodeAnalysis.CSharp.SyntaxKind;

namespace OxidePack.CoreLib.Experimental.Method2Sequence
{
    public class Method2SequenceRewriter : CSharpSyntaxRewriter
    {
        private Dictionary<string, Dictionary<string, TypeSyntax>> _methodsLocals;
        private SemanticModel _semanticModel;
        private Method2SequenceVisitor.Results _info;
        public SyntaxNode Rewrite(SyntaxNode root, Method2SequenceVisitor.Results info)
        {
            _info = info;
            return Visit(root);
        }
        
        public override SyntaxNode VisitLocalDeclarationStatement(LocalDeclarationStatementSyntax node)
        {
            return node.WithDeclaration(node.Declaration.WithType(ParseName("")));
        }
        

        public override SyntaxNode VisitMethodDeclaration(MethodDeclarationSyntax method)
        {
            if (_info.Methods.TryGetValue(method.Identifier.Text, out var srcMethod) &&
                srcMethod.parentClass == ((ClassDeclarationSyntax) method.Parent).Identifier.Text)
            {
                var className = $"{srcMethod.parentClass}_{method.Identifier.Text}_sequence";
                return method.WithBody(
                    Block(
                        GenerateVariable(className, "fuck", $"new {className}()"),
                        ExpressionStatement(InvocationExpression(
                            MemberAccessExpression(SimpleMemberAccessExpression, IdentifierName("fuck"),
                                IdentifierName($"{method.Identifier.Text}_execute")),
                            ArgumentList(
                                SeparatedList(method.ParameterList.Parameters.Select(p =>
                                    Argument(IdentifierName(p.Identifier)))))
                        ))));
            }
            return base.VisitMethodDeclaration(method);
        }
        
        LocalDeclarationStatementSyntax GenerateVariable(string type, string name, string defaultValue)
        {
            return LocalDeclarationStatement(
                VariableDeclaration(
                    ParseTypeName(type),
                    SeparatedList(new[]
                    {
                        VariableDeclarator(name)
                            .WithInitializer(EqualsValueClause(IdentifierName(defaultValue)))
                    })
                ));
        }
    }
}