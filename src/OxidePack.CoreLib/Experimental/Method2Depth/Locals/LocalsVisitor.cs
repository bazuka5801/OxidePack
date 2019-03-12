using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace OxidePack.CoreLib.Method2Depth
{
    using static SyntaxFactory;
    
    public class LocalsVisitor: CSharpSyntaxWalker
    {
        private LocalsVisitorResults _localsVisitorResults;
        private SemanticModel _semanticModel;

        public LocalsVisitorResults Walk(ClassDeclarationSyntax baseClass, SemanticModel semanticModel)
        {
            _localsVisitorResults = new LocalsVisitorResults();
            _semanticModel = semanticModel;

            Visit(baseClass);

            return _localsVisitorResults;
        }

        public override void VisitLocalDeclarationStatement(LocalDeclarationStatementSyntax node)
        {
            var method = node.GetParent<MethodDeclarationSyntax>();
            var mClass = method.GetParent<ClassDeclarationSyntax>();

            var key = $"{mClass.Identifier.Text}.{method.Identifier.Text}";
            if (_localsVisitorResults.MethodsLocals.TryGetValue(key, out var dict) == false)
            {
                _localsVisitorResults.MethodsLocals[key] = dict = new List<(string locName, TypeSyntax locType)>();
            }

            foreach (var variable in node.Declaration.Variables)
            {
                var type = node.Declaration.Type;
                if (type.IsVar)
                {
                    type = ParseTypeName(_semanticModel.GetSymbolInfo(node.Declaration.Type).Symbol.ToString());
                }

                dict.Add((variable.Identifier.Text, type));
            }

            base.VisitLocalDeclarationStatement(node);
        }
    }
}