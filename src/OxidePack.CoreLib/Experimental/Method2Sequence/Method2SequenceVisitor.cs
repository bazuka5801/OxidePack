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
    public class Method2SequenceVisitor : CSharpSyntaxWalker
    {
        public class Results
        {
            /// <summary>
            ///   Method - [locName, locType]
            /// </summary>
            public Dictionary<string, List<(string locName, TypeSyntax locType)>> MethodsLocals;
            
            /// <summary>
            ///   Method - Parent Class
            /// </summary>
            public Dictionary<string, (string parentClass, MethodDeclarationSyntax declaration)> Methods;
            
            public Results()
            {
                this.MethodsLocals = new Dictionary<string, List<(string locName, TypeSyntax locType)>>();
                this.Methods = new Dictionary<string, (string parentClass, MethodDeclarationSyntax declaration)>();
            }

            public bool GetLocals( MethodDeclarationSyntax method, string parentClass , out List<(string locName, TypeSyntax locType)> locals)
            {
                return MethodsLocals.TryGetValue($"{parentClass}.{method.Identifier.Text}", out locals);
            }

            public void Merge(Results results)
            {
                foreach (var methodVar in results.MethodsLocals)
                {
                    MethodsLocals.Add(methodVar.Key, methodVar.Value);
                }
                results.MethodsLocals.Clear();
                
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
        
        public override void VisitLocalDeclarationStatement(LocalDeclarationStatementSyntax node)
        {
            var method = (MethodDeclarationSyntax)node.Parent.Parent;
            var mClass = (ClassDeclarationSyntax)method.Parent;

            var key = $"{mClass.Identifier.Text}.{method.Identifier.Text}";
            if (_results.MethodsLocals.TryGetValue(key, out var dict) == false)
            {
                _results.MethodsLocals[key] = dict = new List<(string locName, TypeSyntax locType)>();
            }
            
            foreach (var variable in node.Declaration.Variables)
            {
                dict.Add( (variable.Identifier.Text, node.Declaration.Type) );
            }
            base.VisitLocalDeclarationStatement(node);
        }

        public override void VisitMethodDeclaration(MethodDeclarationSyntax method)
        {
            _results.Methods[method.Identifier.Text] = (((ClassDeclarationSyntax)method.Parent).Identifier.Text, method);
            base.VisitMethodDeclaration(method);
        }

        public override void VisitClassDeclaration(ClassDeclarationSyntax mClass)
        {
            if (mClass == _baseClass)
            {
                base.VisitClassDeclaration(mClass);
                return;
            }
            
            var visitor = new Method2SequenceVisitor();
            _results.Merge(visitor.Walk(mClass));
        }
    }
}