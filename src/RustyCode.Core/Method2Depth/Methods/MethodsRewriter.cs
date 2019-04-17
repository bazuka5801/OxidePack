using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using OxidePack.CoreLib.Utils;

namespace OxidePack.CoreLib.Method2Depth
{
    using static SyntaxFactory;
    using static SyntaxKind;
    using static SyntaxFactoryUtils;

    public class MethodsRewriter : CSharpSyntaxRewriter
    {
        private static AdhocWorkspace _workspace = new AdhocWorkspace();
        private SyntaxGenerator _generator;
        private MethodsVisitorResults _info;

        public SyntaxNode Rewrite(SyntaxNode root, MethodsVisitorResults info, SyntaxGenerator generator)
        {
            _info = info;
            _generator = generator;

            root = Visit(root);

            return root;
        }

        public override SyntaxNode VisitMethodDeclaration(MethodDeclarationSyntax method)
        {
            if (_info.Methods.TryGetValue(method.FullPath(), out var methodData) &&
                methodData.parentClass == ((ClassDeclarationSyntax) method.Parent).Identifier.Text)
            {
                method = (MethodDeclarationSyntax) base.VisitMethodDeclaration(method);

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
                            LocalVariable(methodData.methodClassName, tempVarName,
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
                        LocalVariable(methodData.methodClassName, tempVarName,
                            $"{methodData.getName}({parametersString})"),
                        ExpressionStatement(InvocationExpression(
                            MemberAccessExpression(SimpleMemberAccessExpression, IdentifierName(tempVarName),
                                IdentifierName($"{methodData.methodClassMethodName}")))),
                        ParseStatement($"{methodData.pushName}({tempVarName});")
                    ));
            }

            return method;
        }
    }
}