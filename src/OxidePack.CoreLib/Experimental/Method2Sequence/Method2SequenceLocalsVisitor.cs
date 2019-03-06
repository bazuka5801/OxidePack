using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Microsoft.CodeAnalysis.CSharp.SyntaxKind;

namespace OxidePack.CoreLib.Experimental.Method2Sequence
{
    public class Method2SequenceLocalsVisitor : CSharpSyntaxWalker
    {
        public class Results
        {
            /// <summary>
            ///   Method - [locName, locType]
            /// </summary>
            public Dictionary<string, List<(string locName, TypeSyntax locType)>> MethodsLocals;


            public Results()
            {
                this.MethodsLocals = new Dictionary<string, List<(string locName, TypeSyntax locType)>>();
            }

            public bool GetLocals( MethodDeclarationSyntax method, string parentClass , out List<(string locName, TypeSyntax locType)> locals)
            {
                return MethodsLocals.TryGetValue($"{parentClass}.{method.Identifier.Text}", out locals);
            }
        }

        private Results _results;
        private SemanticModel _semanticModel;
        
        public Results Walk(ClassDeclarationSyntax baseClass, SemanticModel semanticModel)
        {
            _results = new Results();
            _semanticModel = semanticModel;
            
            Visit(baseClass);
            
            return _results;
        }
        

        public override void VisitLocalDeclarationStatement(LocalDeclarationStatementSyntax node)
        {
            var method = node.GetParent<MethodDeclarationSyntax>();
            var mClass = method.GetParent<ClassDeclarationSyntax>();

            var key = $"{mClass.Identifier.Text}.{method.Identifier.Text}";
            if (_results.MethodsLocals.TryGetValue(key, out var dict) == false)
            {
                _results.MethodsLocals[key] = dict = new List<(string locName, TypeSyntax locType)>();
            }
            
            foreach (var variable in node.Declaration.Variables)
            {
                var type = node.Declaration.Type;
                if (type.IsVar)
                {
                    type = ParseTypeName(_semanticModel.GetSymbolInfo(node.Declaration.Type).Symbol.Name);
                }
                dict.Add( (variable.Identifier.Text, type) );
            }
            base.VisitLocalDeclarationStatement(node);
        }
    }
}