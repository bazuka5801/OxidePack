using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace OxidePack.CoreLib
{
    public partial class Plugin
    {
        private static Regex _HookIdentifierRegex = new Regex( @"^[a-zA-Z0-9_]+__" );
        
        public (SyntaxList<MemberDeclarationSyntax> members, SyntaxList<UsingDirectiveSyntax> usings)
            MergeSources(List<SourceFile> sources)
        {
            var pluginBody = new List<MemberDeclarationSyntax>();
            var usings = new List<UsingDirectiveSyntax>();
            
            // [(method name), (hook)]
            var Hooks = new Dictionary<string, List<MethodDeclarationSyntax>>();

            void AddHook(string hookname, MethodDeclarationSyntax method)
            {
                if (!Hooks.TryGetValue(hookname, out var list))
                    Hooks[hookname] = list = new List<MethodDeclarationSyntax>();
                list.Add(method);
            }

            var repeats = new Dictionary<string, int>();
            string UniqueIdentifier(string name)
            {
                if (repeats.ContainsKey(name))
                {
                    return $"{name}_{++repeats[name]}";
                }
                
                return name;
            }

            foreach (var source in sources)
            {
                foreach (var @using in source.Usings)
                {
                    var name = @using.Name.ToString();
                    if (usings.All(p => p.Name.ToString() != name))
                    {
                        usings.Add(@using);
                    }
                }

                foreach (var sourceMember in source.Members)
                {
                    if (sourceMember is MethodDeclarationSyntax method)
                    {
                        var parameterList = method.ParameterList?.Parameters ?? SyntaxFactory.SeparatedList<ParameterSyntax>();
                        var methodParams = string.Join(", ", (parameterList.Select(p => p.Type.ToString()).ToArray()));
                        var name = method.Identifier.ToString();
                        if (_HookIdentifierRegex.IsMatch(name))
                        {
                            var methodname = UniqueIdentifier(name);
                            if (methodname != name)
                                method = method.WithIdentifier(Identifier(methodname));
                            
                            

                            var hookname = _HookIdentifierRegex.Replace(name, "");
                            AddHook(hookname, method);
                            pluginBody.Add(method);
                            
                            continue;
                        }
                        else
                        {
                            pluginBody.Add(method);
                        }
                    }
                    else
                    {
                        pluginBody.Add(sourceMember);
                    }
                }
            }

            var hookMethods = Hooks.Select(p => CodeGenerator.AddHookMethod(p.Key, p.Value)).ToList();
            pluginBody.AddRange(hookMethods);
            return (List(pluginBody), List(usings));
        }
    }
}