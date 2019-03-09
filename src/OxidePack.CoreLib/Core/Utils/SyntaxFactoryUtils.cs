using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace OxidePack.CoreLib.Utils
{
    using static SyntaxFactory;
    
    public static class SyntaxFactoryUtils
    {
        public static LocalDeclarationStatementSyntax LocalVariable(string type, string name, string defaultValue) =>
            LocalVariable(ParseTypeName(type), name, ParseExpression(defaultValue));
        
        public static LocalDeclarationStatementSyntax LocalVariable(TypeSyntax type, string name, ExpressionSyntax defaultValue) =>
            LocalDeclarationStatement(
                VariableDeclaration(type, SeparatedList(new[]
                    {
                        VariableDeclarator(name)
                            .WithInitializer(EqualsValueClause(defaultValue))
                    })));
    }
}