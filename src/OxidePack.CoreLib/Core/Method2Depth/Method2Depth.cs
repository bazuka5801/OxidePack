using System;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace OxidePack.CoreLib.Method2Depth
{
    public class Method2Depth
    {

        public string ProcessSource(string source, bool controlFlow)
        {
            var tree = new CompilationTree(source, "references/");

            new Core(tree).Process(controlFlow);

            return tree.SourceText;
        }

        public AdhocWorkspace ProcessWorkspace(AdhocWorkspace workspace, EncryptorOptions options = null)
        {
            options = options ?? new EncryptorOptions();
            if (options.Spaghetti)
            {
                foreach (var project in workspace.CurrentSolution.Projects)
                {
                    foreach (var document in project.Documents.ToList())
                    {
                        var tree = document.GetSyntaxTreeAsync().Result;

                        var depth = Math.Max(1, Math.Min(options.SpaghettiDepth, 1));

                        SyntaxNode root = null;
                        for (int i = 0; i < depth; i++)
                        {
                            var compilationTree = new CompilationTree(tree);

                            new Core(compilationTree).Process(options.SpaghettiControlFlow);

                            root = compilationTree.Root;
                        }

                        project.RemoveDocument(document.Id);
                        project.AddDocument("Plugin", root);
                    }
                }
            }

            return workspace;
        }
    }
}