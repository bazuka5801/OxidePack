using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace OxidePack.CoreLib.Experimental.Method2Sequence
{
    public class Method2SequenceSyntaxVisitor : CSharpSyntaxWalker
    {
        private ClassDeclarationSyntax _baseClass;

        private Results _results;

        public Results Walk(ClassDeclarationSyntax baseClass)
        {
            _baseClass = baseClass;
            _results = new Results();

            Visit(baseClass);

            return _results;
        }

        public override void VisitMethodDeclaration(MethodDeclarationSyntax method)
        {
            // Abstart and Extern methods
            if (method.Body == null && method.ExpressionBody == null) return;
            // IEnumerator methods with yield
            if (method.ReturnType.ToString() == "IEnumerator") return;
            // <T> Classes
            if (method.GetParent<ClassDeclarationSyntax>().ConstraintClauses.Count > 0) return;
            
            var parentClassName = method.GetParent<ClassDeclarationSyntax>().Identifier.Text;
            _results.Methods[method.FullPath()] = new Results.ClassData
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

            var visitor = new Method2SequenceSyntaxVisitor();
            _results.Merge(visitor.Walk(mClass));
        }

        public class Results
        {
            /// <summary>
            ///     Method - Parent Class
            /// </summary>
            public Dictionary<string, ClassData> Methods;

            public Results()
            {
                Methods = new Dictionary<string, ClassData>();
            }

            public void Merge(Results results)
            {
                foreach (var method in results.Methods)
                {
                    Methods[method.Key] = method.Value;
                }

                results.Methods.Clear();
            }

            public class ClassData
            {
                public MethodDeclarationSyntax declaration;
                public string getName = IdentifierGenerator.GetSimpleName();
                public string methodClassInitializeMethodName = IdentifierGenerator.GetSimpleName();
                public string methodClassMethodName = IdentifierGenerator.GetSimpleName();
                public string methodClassName = IdentifierGenerator.GetSimpleName();
                public string parentClass;
                public string pushName = IdentifierGenerator.GetSimpleName();
            }
        }
    }
}