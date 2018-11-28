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
    public partial class CodeGenerator
    {
        public static MemberDeclarationSyntax AddHookMethod(string hookname, List<MethodDeclarationSyntax> methods)
        {
            bool hasReturn = methods.Any(p => p.ReturnType.ToString() != "void");
            TypeSyntax retType = ParseTypeName(hasReturn ? "object" : "void");
            var parameters = methods
                .OrderByDescending(p => p.ParameterList.Parameters.Count)
                .First().ParameterList;

            return MethodDeclaration(retType, hookname)
                .WithParameterList(parameters)
                .WithBody(GenerateBody());


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

                    List<StatementSyntax> Statements = new List<StatementSyntax>();
                    Statements.Add(GenerateVariable("object", "ret", "null"));
                    Statements.Add(GenerateVariable("object", "temp", "null"));

                    var temp = IdentifierName("temp");
                    var ret = IdentifierName("ret");
                    foreach (var method in methods)
                    {
                        if (method.ReturnType.ToString() != "void")
                        {
                            Statements.Add(ExpressionStatement(AssignmentExpression(SimpleAssignmentExpression,
                                IdentifierName("temp"), GenerateCallHook(method))));
                            Statements.Add(IfStatement(
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
                            Statements.Add(ExpressionStatement(GenerateCallHook(method)));
                        }
                    }
                    Statements.Add(ReturnStatement(ret));

                    return Block(Statements);
                }
                else
                {
                    return Block(methods.Select(method=> ExpressionStatement(GenerateCallHook(method))));
                }

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