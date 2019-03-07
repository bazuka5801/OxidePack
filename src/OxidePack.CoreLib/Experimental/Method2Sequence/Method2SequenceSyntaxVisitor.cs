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
    public class Method2SequenceSyntaxVisitor : CSharpSyntaxWalker
    {
        public class Results
        {
            /// <summary>
            ///   Method - Parent Class
            /// </summary>
            public Dictionary<string, ClassData> Methods;
            
            public class ClassData
            {
                public string parentClass;
                public MethodDeclarationSyntax declaration;
                public string getName = IdentifierGenerator.GetSimpleName();
                public string pushName = IdentifierGenerator.GetSimpleName();
                public string methodClassName = IdentifierGenerator.GetSimpleName();
                public string methodClassMethodName = IdentifierGenerator.GetSimpleName();
                public string methodClassInitializeMethodName = IdentifierGenerator.GetSimpleName();
            }
            
            public Results()
            {
                this.Methods = new Dictionary<string, ClassData>();
            }

            public void Merge(Results results)
            {
                foreach (var method in results.Methods)
                {
                    Methods[method.Key] = method.Value;
                }
                results.Methods.Clear();
            }
        }

        private Results _results;
        private ClassDeclarationSyntax _baseClass;
        
        public Results Walk(ClassDeclarationSyntax baseClass)
        {
            _baseClass = baseClass;
            _results = new Results();

            Visit(baseClass);
            
            return _results;
        }

        public override void VisitMethodDeclaration(MethodDeclarationSyntax method)
        {
            var parentClassName = method.GetParent<ClassDeclarationSyntax>().Identifier.Text;
            _results.Methods[method.FullPath()] = new Results.ClassData()
            {
                declaration = method,
                parentClass = ((ClassDeclarationSyntax)method.Parent).Identifier.Text
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
    }
}