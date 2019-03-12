// <Minified names>

using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MemberList = Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax>;
// </Minified names>
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Microsoft.CodeAnalysis.CSharp.SyntaxKind;

namespace OxidePack.CoreLib
{
    public class CodeGenerator
    {
        public static MemberDeclarationSyntax AddHookMethod(string hookname, List<MethodDeclarationSyntax> methods,
            bool prepareEncrypt)
        {
            var hasReturn = methods.Any(p => p.ReturnType.ToString() != "void");
            var retType = ParseTypeName(hasReturn ? "object" : "void");
            var parameters = methods
                .OrderByDescending(p => p.ParameterList.Parameters.Count)
                .First().ParameterList;

            var methodResult = MethodDeclaration(retType, hookname)
                .WithParameterList(parameters)
                .WithBody(GenerateBody());
            if (prepareEncrypt)
            {
                var name = ParseName("Oxide.Core.Plugins.HookMethod");
                var arguments = ParseAttributeArgumentList($"(\"{hookname}\")");
                methodResult = methodResult.WithAttributeLists(List(new[]
                {
                    AttributeList(SingletonSeparatedList(Attribute(name, arguments)))
                }));
            }

            return methodResult;

            BlockSyntax GenerateBody()
            {
                if (hasReturn)
                {
                    // object fuckyou(baseplayer player)
                    // {
                    //  object ret = null;
                    //  object temp = null;
                    //  temp = fuckyou1(player);
                    //  if (temp != null) ret = temp;
                    //  return ret;
                    // }

                    var statements = new List<StatementSyntax>();
                    statements.Add(GenerateVariable("object", "ret", "null"));
                    statements.Add(GenerateVariable("object", "temp", "null"));

                    var temp = IdentifierName("temp");
                    var ret = IdentifierName("ret");
                    foreach (var method in methods)
                    {
                        if (method.ReturnType.ToString() != "void")
                        {
                            statements.Add(ExpressionStatement(AssignmentExpression(SimpleAssignmentExpression,
                                IdentifierName("temp"), GenerateCallHook(method))));
                            statements.Add(IfStatement(
                                BinaryExpression(NotEqualsExpression,
                                    temp,
                                    LiteralExpression(NullLiteralExpression)),
                                ExpressionStatement(
                                    AssignmentExpression(
                                        SimpleAssignmentExpression,
                                        ret,
                                        temp))));
                        }
                        else
                        {
                            statements.Add(ExpressionStatement(GenerateCallHook(method)));
                        }
                    }

                    statements.Add(ReturnStatement(ret));

                    return Block(statements);
                }

                return Block(methods.Select(method => ExpressionStatement(GenerateCallHook(method))));

                InvocationExpressionSyntax GenerateCallHook(MethodDeclarationSyntax method)
                {
                    return
                        InvocationExpression(
                            IdentifierName(method.Identifier),
                            ArgumentList(
                                SeparatedList(
                                    parameters.Parameters.Select(p =>
                                        Argument(IdentifierName(p.Identifier))))));
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
    }
}