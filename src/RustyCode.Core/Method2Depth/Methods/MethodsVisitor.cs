using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace OxidePack.CoreLib.Method2Depth
{
    public class MethodsVisitor : CSharpSyntaxWalker
    {
        private ClassDeclarationSyntax _baseClass;
        private SemanticModel _semanticModel;
        private MethodsVisitorResults _methodsVisitorResults;

        public MethodsVisitorResults Walk(ClassDeclarationSyntax baseClass, SemanticModel semanticModel)
        {
            _baseClass = baseClass;
            _semanticModel = semanticModel;
            _methodsVisitorResults = new MethodsVisitorResults();

            Visit(baseClass);

            return _methodsVisitorResults;
        }

        public override void VisitMethodDeclaration(MethodDeclarationSyntax method)
        {
            // Abstart and Extern methods
            if (method.Body == null && method.ExpressionBody == null)
            {
                return;
            }

            // IEnumerator methods with yield
            if (method.ReturnType.ToString() == "IEnumerator")
            {
                return;
            }

            // <T> Classes
            if (method.GetParent<ClassDeclarationSyntax>()?.TypeParameterList?.Parameters.Count > 0)
            {
                return;
            }

            // <T> Methods
            if (method.TypeParameterList?.Parameters.Count > 0)
            {
                return;
            }

            // Methods with out parameters
            if (method.ParameterList.Parameters.Any(p => p.Modifiers.Any(m => m.IsKind(SyntaxKind.OutKeyword))))
            {
                return;
            }


            _methodsVisitorResults.Methods[method.FullPath()] = new MethodClassData
            {
                declaration = method,
                parentClass = ((ClassDeclarationSyntax) method.Parent).Identifier.Text
            };

            base.VisitMethodDeclaration(method);
        }

        public override void VisitClassDeclaration(ClassDeclarationSyntax mClass)
        {
            if (mClass == _baseClass)
            {
                base.VisitClassDeclaration(mClass);
                return;
            }

            var visitor = new MethodsVisitor();
            _methodsVisitorResults.Merge(visitor.Walk(mClass, _semanticModel));
        }

        public override void VisitBaseExpression(BaseExpressionSyntax node)
        {
            base.VisitBaseExpression(node);
            var method = node.GetParent<MethodDeclarationSyntax>();
            if (method == null)
            {
                return;
            }

            if (method.HasModifier(SyntaxKind.OverrideKeyword))
            {
                var baseMethodName = node.GetParent<MemberAccessExpressionSyntax>()?.Name;
                if (baseMethodName.Identifier.Text == method.Identifier.Text)
                {
                    _methodsVisitorResults.Methods.Remove(method.FullPath());
                }
            }
        }
    }
}