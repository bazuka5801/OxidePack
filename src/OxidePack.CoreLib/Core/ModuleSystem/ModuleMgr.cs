using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace OxidePack.CoreLib
{
    public static class ModuleMgr
    {
        private static Dictionary<string, Module> _modules;

        public static void Init()
        {
            LoadModules();
        }

        private static void LoadModules()
        {
            _modules = Directory.GetDirectories(Path.Combine(Directory.GetCurrentDirectory(), "modules"))
                .Select(directory => new Module(directory))
                .ToDictionary(m => m.Manifest.Name, m => m);
        }

        public static List<Module> GetModuleList() => _modules.Values.ToList();

        public static bool GetModule(string name, out Module module) => _modules.TryGetValue(name, out module);

        public static (SyntaxList<MemberDeclarationSyntax> members, SyntaxList<UsingDirectiveSyntax> usings)
            CombineModules(IEnumerable<Module> modules)
        {
            var members = new List<MemberDeclarationSyntax>();
            var usings = new List<UsingDirectiveSyntax>();
            foreach (var module in modules)
            {
                members.AddRange(module.Members);
                foreach (var @using in module.Usings)
                {
                    var name = @using.Name.ToString();
                    if (usings.All(p => p.Name.ToString() != name))
                    {
                        usings.Add(@using);
                    }
                }
            }

            return (new SyntaxList<MemberDeclarationSyntax>(members), new SyntaxList<UsingDirectiveSyntax>(usings));
        }
    }
}