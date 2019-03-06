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
        private Method2SequenceSyntaxVisitor.Results _info;
        public SyntaxNode Rewrite(SyntaxNode root, Method2SequenceSyntaxVisitor.Results info)
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
            var parentClassName = method.GetParent<ClassDeclarationSyntax>().Identifier.Text;
            if (_info.Methods.TryGetValue(parentClassName+"."+method.Identifier.Text, out var methodData) &&
                methodData.parentClass == ((ClassDeclarationSyntax) method.Parent).Identifier.Text)
            {
                var parameters= method.ParameterList.Parameters.Select(p =>
                    Argument(IdentifierName(p.GetFullParameter()))).ToList();
                if (method.Modifiers.Any(p=>p.IsKind(StaticKeyword)) == false)
                    parameters.Insert(0, Argument(IdentifierName("this")));
                if (method.ParameterList.Parameters.Any(p => p.Modifiers.Any(z=>z.IsKind(RefKeyword))))
                {
                    
                }
                var parametersString =
                    string.Join(", ", parameters.Select(p => p.ToString()));
                var tempVarName = IdentifierGenerator.GetSimpleName();

                if (method.ReturnType.ToString() != "void")
                {
                    var catchException = CatchDeclaration(ParseTypeName("System.Exception"))
                        .WithIdentifier(ParseToken("_exception"));
                    var catchBlock = Block(ThrowStatement(IdentifierName("_exception")));
                    return method.WithBody(
                        Block(
                            GenerateVariable(methodData.methodClassName, tempVarName, $"{methodData.getName}({parametersString})"),
                            TryStatement(Block(
                            ReturnStatement(InvocationExpression(
                                MemberAccessExpression(SimpleMemberAccessExpression, IdentifierName(tempVarName),
                                    IdentifierName($"{method.Identifier.Text}_execute"))))
                            ), List(new []{ CatchClause(catchException,default, catchBlock) }), FinallyClause(Block(
                            ParseStatement($"{methodData.pushName}({tempVarName});")
                            )))
                        ));
                }
                else
                {
                    return method.WithBody(
                        Block(GenerateVariable(methodData.methodClassName, tempVarName, $"{methodData.getName}({parametersString})"),
                            ExpressionStatement(InvocationExpression(
                                MemberAccessExpression(SimpleMemberAccessExpression, IdentifierName(tempVarName),
                                    IdentifierName($"{method.Identifier.Text}_execute")))),
                            ParseStatement($"{methodData.pushName}({tempVarName});")
                        ));
                }
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