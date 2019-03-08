using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Microsoft.CodeAnalysis.CSharp.SyntaxKind;

namespace OxidePack.CoreLib.Experimental.Method2Sequence
{
    public class Method2SequenceRewriter : CSharpSyntaxRewriter
    {
        private Method2SequenceSyntaxVisitor.Results _info;

        public SyntaxNode Rewrite(SyntaxNode root, Method2SequenceSyntaxVisitor.Results info)
        {
            _info = info;
            return Visit(root);
        }

        public override SyntaxNode VisitLocalDeclarationStatement(LocalDeclarationStatementSyntax node)
        {
            node = node.WithDeclaration(node.Declaration.WithType(ParseName(""))
                .WithVariables(SeparatedList(node.Declaration.Variables.Where(p => p.Initializer != null))));
            if (node.Declaration.Variables.Count == 0)
            {
                return EmptyStatement();
            }

            return node;
        }


        public override SyntaxNode VisitMethodDeclaration(MethodDeclarationSyntax method)
        {
            if (_info.Methods.TryGetValue(method.FullPath(), out var methodData) &&
                methodData.parentClass == ((ClassDeclarationSyntax) method.Parent).Identifier.Text)
            {
                var parameters = method.ParameterList.Parameters.Select(p =>
                    Argument(IdentifierName(p.GetFullParameter()))).ToList();
                if (method.Modifiers.Any(p => p.IsKind(StaticKeyword)) == false)
                {
                    parameters.Insert(0, Argument(IdentifierName("this")));
                }

                if (method.ParameterList.Parameters.Any(p => p.Modifiers.Any(z => z.IsKind(RefKeyword))))
                {
                }

                var parametersString =
                    string.Join(", ", parameters.Select(p => p.ToString()));
                var tempVarName = IdentifierGenerator.GetSimpleName();

                if (method.ReturnType.ToString() != "void")
                {
                    var catchException = CatchDeclaration(ParseTypeName("System.Exception"))
                        .WithIdentifier(ParseToken("_exception"));
                    var catchBlock = Block(
                        ParseStatement(
                            "global::Oxide.Core.Interface.Oxide.LogError(_exception.Message+\"\\n\"+_exception.StackTrace);"),
                        ThrowStatement());
                    return method.WithBody(
                        Block(
                            Variable(methodData.methodClassName, tempVarName,
                                $"{methodData.getName}({parametersString})"),
                            TryStatement(Block(
                                ReturnStatement(InvocationExpression(
                                    MemberAccessExpression(SimpleMemberAccessExpression, IdentifierName(tempVarName),
                                        IdentifierName($"{methodData.methodClassMethodName}"))))
                            ), List(new[] {CatchClause(catchException, default, catchBlock)}), FinallyClause(Block(
                                ParseStatement($"{methodData.pushName}({tempVarName});")
                            )))
                        ));
                }

                return method.WithBody(
                    Block(
                        Variable(methodData.methodClassName, tempVarName,
                            $"{methodData.getName}({parametersString})"),
                        ExpressionStatement(InvocationExpression(
                            MemberAccessExpression(SimpleMemberAccessExpression, IdentifierName(tempVarName),
                                IdentifierName($"{methodData.methodClassMethodName}")))),
                        ParseStatement($"{methodData.pushName}({tempVarName});")
                    ));
            }

            return base.VisitMethodDeclaration(method);
        }

        private LocalDeclarationStatementSyntax Variable(string type, string name, string defaultValue) =>
            LocalDeclarationStatement(
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