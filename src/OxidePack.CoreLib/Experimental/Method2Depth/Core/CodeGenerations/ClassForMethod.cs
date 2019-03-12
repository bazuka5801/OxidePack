using System.Collections;
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
        class MethodClassGeneration
        {
            private MethodClassData _methodClassData;
            private LocalsVisitorResults _localsVisitorResults;
            private ThisVisitorResults _thisVisitorResults;
            private string _nullStructName;
            private SyntaxGenerator _generator;

            private MethodDeclarationSyntax _method;
            private string _thisKeyword;

            private List<MemberDeclarationSyntax> _members;

            /// <summary>
            ///   Create an instance of MethodClassGeneration
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
            ///   Generate a class for method execution
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
            }


            /// <summary>
            ///   Create fields from [this(if not static), parameters, locals] 
            /// </summary>
            /// <param name="initializingParameters">[this(if not static), parameters]</param>
            /// <returns>List with fields</returns>
            private List<FieldDeclarationSyntax> Fields(IEnumerable<ParameterSyntax> initializingParameters)
            {
                var fields = new List<FieldDeclarationSyntax>();
                foreach (var parameter in initializingParameters)
                {
                    var field = (FieldDeclarationSyntax)_generator.FieldDeclaration(
                        name: parameter.Identifier.Text,
                        type: parameter.Type,
                        accessibility: Accessibility.Public
                        );

                    fields.Add(field);
                }

                if (_localsVisitorResults.GetLocals(_method, _methodClassData.parentClass, out var locals))
                {
                    foreach (var local in locals)
                    {
                        var field = (FieldDeclarationSyntax)_generator.FieldDeclaration(
                            name: local.locName,
                            type: local.locType,
                            accessibility: Accessibility.Public
                        );

                        fields.Add(field);
                    }
                }

                return fields;
            }


            /// <summary>
            ///   Initializing parameters [this(if not static), parameters]
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
            ///   public void Initialize(t1 arg1, t2 arg2)
            ///   {
            ///       this.arg1 = arg1;
            ///       this.arg2 = arg2;
            ///   }
            ///
            ///   If method is not static, then insert(0, _this)
            /// </summary>
            /// <returns></returns>
            private MethodDeclarationSyntax InitializeMethod(SeparatedSyntaxList<ParameterSyntax> parameters)
            {
                var initializeMethod = (MethodDeclarationSyntax)_generator.MethodDeclaration(
                    name: _methodClassData.methodClassInitializeMethodName,
                    returnType: _generator.TypeExpression(SpecialType.System_Void),
                    accessibility: Accessibility.Public,
                    parameters: parameters,
                    statements: parameters.Select(p =>
                        _generator.AssignmentStatement(
                            left:  _generator.MemberAccessExpression(ThisExpression(), p.Identifier.Text),
                            right: IdentifierName(p.Identifier)
                            )
                        )
                    );

                return initializeMethod;
            }


            /// <summary>
            ///   public ReturnType execute()
            ///   {
            ///       ... statements ...
            ///   }
            /// </summary>
            /// <returns>Execute method</returns>
            private MethodDeclarationSyntax ExecuteMethod()
            {
                var methodBody = _method.Body?.Statements.ToList() ?? new List<StatementSyntax>
                                     {ExpressionStatement(_method.ExpressionBody.Expression)};

                SeparateStatements(methodBody);

                var executeMethod = (MethodDeclarationSyntax)_generator.MethodDeclaration(
                    name:          _methodClassData.methodClassMethodName,
                    returnType:    _method.ReturnType,
                    accessibility: Accessibility.Public,
                    statements:    methodBody
                    );

                return executeMethod;
            }


            /// <summary>
            ///   Separate statements to multiple methods
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
                                (FieldDeclarationSyntax)_generator.FieldDeclaration(
                                    name: returnTemp.Identifier.Text,
                                    type: _generator.TypeExpression(SpecialType.System_Object),
                                    accessibility: Accessibility.Public
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