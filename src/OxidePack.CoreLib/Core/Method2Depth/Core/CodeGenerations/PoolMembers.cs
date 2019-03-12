using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;

namespace OxidePack.CoreLib.Method2Depth
{
    using static SyntaxFactory;
    using static SyntaxKind;

    internal partial class Core
    {
        private class PoolMembersGeneration
        {
            private readonly SyntaxGenerator _generator;
            private readonly List<MemberDeclarationSyntax> _members;
            private readonly MethodClassData _methodClassData;
            private readonly ThisVisitorResults _thisInfo;

            public PoolMembersGeneration(List<MemberDeclarationSyntax> members,
                MethodClassData methodClassData, ThisVisitorResults thisInfo, SyntaxGenerator generator)
            {
                _members = members;
                _methodClassData = methodClassData;
                _thisInfo = thisInfo;
                _generator = generator;
            }

            /// <summary>
            ///     Generate the pool members
            /// </summary>
            public void Generate()
            {
                var listName = IdentifierGenerator.GetSimpleName();
                CreateList(listName);
                PushMethod(listName);
                GetMethod(listName);
            }

            /// <summary>
            ///     Create a pool collection of MethodClass
            /// </summary>
            /// <param name="identifier">Identifier of pool collection</param>
            private void CreateList(string identifier)
            {
                var listType = $"System.Collections.Generic.List<{_methodClassData.methodClassName}>";
                var list = (FieldDeclarationSyntax) _generator.FieldDeclaration(
                    identifier,
                    ParseTypeName(listType),
                    modifiers: DeclarationModifiers.Static,
                    initializer: ParseExpression(
                        $"new System.Collections.Generic.List<{_methodClassData.methodClassName}>()")
                );

                _members.Add(list);
            }

            /// <summary>
            ///     Create a push method to push MethodClass after use into pool
            /// </summary>
            /// <param name="listIdentifier">Identifier of pool collection</param>
            private void PushMethod(string listIdentifier)
            {
                var parameterName = IdentifierGenerator.GetSimpleName();
                var pushMethod = (MethodDeclarationSyntax) _generator.MethodDeclaration(
                    _methodClassData.pushName,
                    returnType: ParseTypeName("void"),
                    modifiers: DeclarationModifiers.Static,
                    parameters: new[]
                    {
                        _generator.ParameterDeclaration(
                            parameterName,
                            ParseTypeName(_methodClassData.methodClassName))
                    },
                    statements: new[]
                    {
                        ExpressionStatement(
                            InvocationExpression(
                                (MemberAccessExpressionSyntax) _generator.MemberAccessExpression(
                                    IdentifierName(listIdentifier),
                                    "Add"
                                ),
                                ArgumentList(SeparatedList(new[]
                                {
                                    Argument(IdentifierName(parameterName))
                                }))
                            )
                        )
                    }
                );

                _members.Add(pushMethod);
            }

            /// <summary>
            ///     Create a get method to get MethodClass from pool or create new if pool is empty
            /// </summary>
            /// <param name="listIdentifier">Identifier of pool collection</param>
            private void GetMethod(string listIdentifier)
            {
                var method = _methodClassData.declaration;
                var parameters = method.ParameterList.Parameters.ToList();
                if (!method.HasModifier(StaticKeyword))
                {
                    var thisName = _thisInfo.ThisNames[method.FullPath()];
                    parameters.Insert(0,
                        Parameter(Identifier(thisName))
                            .WithType(ParseTypeName(_methodClassData.parentClass)));
                }

                var parametersLine = string.Join(", ", parameters
                    .Select(p => p.GetFullParameter()).ToArray());

                var tempIdentifier = IdentifierGenerator.GetSimpleName();
                var initializeMethodName = _methodClassData.methodClassInitializeMethodName;
                var className = _methodClassData.methodClassName;
                var pushMethod = (MethodDeclarationSyntax) _generator.MethodDeclaration(
                    _methodClassData.getName,
                    returnType: ParseTypeName(className),
                    modifiers: DeclarationModifiers.Static,
                    parameters: parameters,
                    statements: new[]
                    {
                        // MethodClassName temp;
                        _generator.LocalDeclarationStatement(
                            ParseTypeName(className),
                            tempIdentifier
                        ),

                        // if (list.Count > 0)
                        IfStatement(ParseExpression($"{listIdentifier}.Count > 0"), Block(ExpressionStatement(
                                AssignmentExpression(
                                    SimpleAssignmentExpression,
                                    IdentifierName($"{tempIdentifier}"),
                                    ParseExpression($"{listIdentifier}[0]"))
                            ), ParseStatement($"{listIdentifier}.RemoveAt(0);"), ParseStatement(
                                $"{tempIdentifier}.{initializeMethodName}({parametersLine});"),
                            ReturnStatement(IdentifierName($"{tempIdentifier}")))),

                        // temp = new MethodClassName();
                        ExpressionStatement(AssignmentExpression(
                            SimpleAssignmentExpression,
                            IdentifierName(tempIdentifier),
                            ParseExpression($"new {className}()")
                        )),

                        // temp.Initialize(parameters);
                        ParseStatement($"{tempIdentifier}.{initializeMethodName}({parametersLine});"),

                        // return temp;
                        ReturnStatement(IdentifierName(tempIdentifier))
                    }
                );

                _members.Add(pushMethod);
            }
        }
    }
}