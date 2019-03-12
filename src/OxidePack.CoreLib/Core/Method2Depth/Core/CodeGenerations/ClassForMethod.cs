using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;

namespace OxidePack.CoreLib.Method2Depth
{
    using static SyntaxFactory;
    using static SyntaxKind;

    internal partial class Core
    {
        private class MethodClassGeneration
        {
            private readonly SyntaxGenerator _generator;
            private readonly LocalsVisitorResults _localsVisitorResults;

            private readonly List<MemberDeclarationSyntax> _members;

            private MethodDeclarationSyntax _method;
            private readonly MethodClassData _methodClassData;
            private readonly string _nullStructName;
            private string _thisKeyword;
            private readonly ThisVisitorResults _thisVisitorResults;

            /// <summary>
            ///     Create an instance of MethodClassGeneration
            /// </summary>
            /// <param name="methodClassData">MethodClassData</param>
            /// <param name="localsVisitorResults">Results of LocalsVisitor</param>
            /// <param name="thisVisitorResults">Results of ThisVisitor</param>
            /// <param name="nullStructName">Name of NullStruct</param>
            /// <param name="generator">SyntaxGenerator</param>
            public MethodClassGeneration(MethodClassData methodClassData, LocalsVisitorResults localsVisitorResults,
                ThisVisitorResults thisVisitorResults, string nullStructName, SyntaxGenerator generator)
            {
                _methodClassData = methodClassData;
                _localsVisitorResults = localsVisitorResults;
                _thisVisitorResults = thisVisitorResults;
                _nullStructName = nullStructName;
                _generator = generator;
                _members = new List<MemberDeclarationSyntax>();
            }

            /// <summary>
            ///     Generate a class for method execution
            /// </summary>
            /// <returns></returns>
            public ClassDeclarationSyntax Generate()
            {
                _method = _methodClassData.declaration;
                _thisKeyword = _thisVisitorResults.ThisNames[_method.FullPath()];

                var initializingParameters = InitializingParameters();
                _members.AddRange(Fields(initializingParameters));
                _members.Add(InitializeMethod(initializingParameters));
                _members.Add(ExecuteMethod());

                var methodClass = (ClassDeclarationSyntax) _generator.ClassDeclaration(
                    _methodClassData.methodClassName,
                    members: _members
                );

                return methodClass;
            }


            /// <summary>
            ///     Create fields from [this(if not static), parameters, locals]
            /// </summary>
            /// <param name="initializingParameters">[this(if not static), parameters]</param>
            /// <returns>List with fields</returns>
            private List<FieldDeclarationSyntax> Fields(IEnumerable<ParameterSyntax> initializingParameters)
            {
                var fields = new List<FieldDeclarationSyntax>();
                foreach (var parameter in initializingParameters)
                {
                    var field = (FieldDeclarationSyntax) _generator.FieldDeclaration(
                        parameter.Identifier.Text,
                        parameter.Type,
                        Accessibility.Public
                    );

                    fields.Add(field);
                }

                if (_localsVisitorResults.GetLocals(_method, _methodClassData.parentClass, out var locals))
                {
                    foreach (var local in locals)
                    {
                        var field = (FieldDeclarationSyntax) _generator.FieldDeclaration(
                            local.locName,
                            local.locType,
                            Accessibility.Public
                        );

                        fields.Add(field);
                    }
                }

                return fields;
            }


            /// <summary>
            ///     Initializing parameters [this(if not static), parameters]
            /// </summary>
            /// <returns>SeparatedList with parameters</returns>
            private SeparatedSyntaxList<ParameterSyntax> InitializingParameters()
            {
                var parameters = SeparatedList(_method.ParameterList.Parameters);
                if (_method.HasModifier(StaticKeyword) == false)
                {
                    parameters = parameters.Insert(0,
                        Parameter(Identifier(_thisKeyword))
                            .WithType(ParseTypeName(_methodClassData.parentClass)));
                }

                return parameters;
            }


            /// <summary>
            ///     public void Initialize(t1 arg1, t2 arg2)
            ///     {
            ///     this.arg1 = arg1;
            ///     this.arg2 = arg2;
            ///     }
            ///     If method is not static, then insert(0, _this)
            /// </summary>
            /// <returns></returns>
            private MethodDeclarationSyntax InitializeMethod(SeparatedSyntaxList<ParameterSyntax> parameters)
            {
                var initializeMethod = (MethodDeclarationSyntax) _generator.MethodDeclaration(
                    _methodClassData.methodClassInitializeMethodName,
                    returnType: ParseTypeName("void"),
                    accessibility: Accessibility.Public,
                    parameters: parameters,
                    statements: parameters.Select(p =>
                        _generator.AssignmentStatement(
                            _generator.MemberAccessExpression(ThisExpression(), p.Identifier.Text),
                            IdentifierName(p.Identifier)
                        )
                    )
                );

                return initializeMethod;
            }


            /// <summary>
            ///     public ReturnType execute()
            ///     {
            ///     ... statements ...
            ///     }
            /// </summary>
            /// <returns>Execute method</returns>
            private MethodDeclarationSyntax ExecuteMethod()
            {
                var methodBody = _method.Body?.Statements.ToList() ?? new List<StatementSyntax>
                                     {ExpressionStatement(_method.ExpressionBody.Expression)};

                SeparateStatements(methodBody);

                var executeMethod = (MethodDeclarationSyntax) _generator.MethodDeclaration(
                    _methodClassData.methodClassMethodName,
                    returnType: _method.ReturnType,
                    accessibility: Accessibility.Public,
                    statements: methodBody
                );

                return executeMethod;
            }


            /// <summary>
            ///     Separate statements to multiple methods
            /// </summary>
            /// <param name="statements">Statements</param>
            private void SeparateStatements(List<StatementSyntax> statements)
            {
                var returnType = _method.ReturnType;
                IdentifierNameSyntax returnTemp = null;
                for (var i = statements.Count - 1; i >= 0; i--)
                {
                    var statement = statements[i];
                    statement = (StatementSyntax) new ReturnRewriter().Rewrite(statement, returnType,
                        _nullStructName);
                    var methodName = IdentifierGenerator.GetSimpleName();

                    var hasReturn = new ReturnSearcher().Search(statement);

                    var methodStatements = new List<StatementSyntax>
                    {
                        statement
                    };

                    if (statement.IsKind(SyntaxKind.ReturnStatement) == false)
                    {
                        methodStatements.Add(ReturnStatement(IdentifierName("null")));
                    }

                    _members.Add(MethodDeclaration(ParseTypeName("object"), methodName)
                        .WithModifiers(TokenList(Token(PublicKeyword)))
                        .WithBody(Block(methodStatements)));

                    if (hasReturn)
                    {
                        if (returnType.ToString() == "void")
                        {
                            statements[i] = IfStatement(ParseExpression($"{methodName}() is {_nullStructName}"),
                                ReturnStatement());
                            continue;
                        }

                        if (returnTemp == null)
                        {
                            returnTemp = IdentifierName(IdentifierGenerator.GetSimpleName());
                            _members.Add(
                                (FieldDeclarationSyntax) _generator.FieldDeclaration(
                                    returnTemp.Identifier.Text,
                                    _generator.TypeExpression(SpecialType.System_Object),
                                    Accessibility.Public
                                )
                            );
                        }

                        if (i == statements.Count - 1)
                        {
                            statements.RemoveAt(i);
                            statements.InsertRange(i, new StatementSyntax[]
                            {
                                ExpressionStatement(AssignmentExpression(SimpleAssignmentExpression,
                                    returnTemp,
                                    InvocationExpression(IdentifierName(methodName)))),
                                IfStatement(ParseExpression($"{returnTemp} is {_nullStructName}"),
                                    ReturnStatement(IdentifierName($"default({returnType.ToString()})"))),
                                returnType.ToString() == "object"
                                    ? ReturnStatement(returnTemp)
                                    : ReturnStatement(CastExpression(returnType, returnTemp))
                            });
                            continue;
                        }

                        statements.RemoveAt(i);
                        statements.InsertRange(i, new StatementSyntax[]
                        {
                            ExpressionStatement(AssignmentExpression(SimpleAssignmentExpression,
                                returnTemp,
                                InvocationExpression(IdentifierName(methodName)))),
                            IfStatement(ParseExpression($"{returnTemp} is {_nullStructName}"),
                                ReturnStatement(IdentifierName($"default({returnType.ToString()})"))),
                            IfStatement(ParseExpression($"{returnTemp} != null"),
                                ReturnStatement(returnType.ToString() == "object"
                                    ? (ExpressionSyntax) returnTemp
                                    : CastExpression(returnType, returnTemp)))
                        });
                    }
                    else
                    {
                        statements[i] = ExpressionStatement(InvocationExpression(IdentifierName(methodName)));
                    }
                }
            }
        }
    }
}