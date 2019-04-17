using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Formatting;
using Microsoft.CodeAnalysis.Editing;
using Microsoft.CodeAnalysis.Formatting;

namespace OxidePack.CoreLib
{
    internal class CompilationTree
    {
        private static readonly AdhocWorkspace _workspace = new AdhocWorkspace();
        private static SyntaxGenerator _generator;

        private readonly List<ICompilationTreeSubscriber> _subscribers;

        private string _assemblyName = "CoreLib";
        private CSharpCompilation _compilation;
        private SyntaxNode _root;
        private SyntaxTree _tree;

        /// <summary>
        ///     Create a new instance of CompilationTree with SourceText and ReferencesPath
        /// </summary>
        /// <param name="sourceText">SourceText</param>
        /// <param name="referencesPath">ReferencesPath</param>
        public CompilationTree(string sourceText, string referencesPath = "references/")
        {
            _subscribers = new List<ICompilationTreeSubscriber>();

            UpdateCompilation(CSharpCompilation.Create("CoreLib"));
            UpdateReferences(referencesPath);
            UpdateSourceText(sourceText);
        }

        /// <summary>
        ///     Create a new instance of CompilationTree with SyntaxTree and ReferencesPath
        /// </summary>
        /// <param name="tree">SyntaxTree</param>
        /// <param name="referencesPath">ReferencesPath</param>
        public CompilationTree(SyntaxTree tree, string referencesPath = "references/")
        {
            _subscribers = new List<ICompilationTreeSubscriber>();

            UpdateCompilation(CSharpCompilation.Create("CoreLib"));
            UpdateReferences(referencesPath);
            UpdateTree(tree, true);
        }


        /// <summary>
        ///     Syntax Tree what contains root node
        /// </summary>
        public SyntaxTree Tree
        {
            get => _tree;
            set => UpdateTree(value, true);
        }

        /// <summary>
        ///     Root node
        /// </summary>
        public SyntaxNode Root
        {
            get => _root;
            set => UpdateRoot(value, true);
        }

        /// <summary>
        ///     Source Text
        /// </summary>
        public string SourceText
        {
            get => GetSourceText();
            set => UpdateSourceText(value);
        }

        /// <summary>
        ///     Semantic model for get more information about nodes
        /// </summary>
        public SemanticModel SemanticModel { get; private set; }

        /// <summary>
        ///     Get static workspace
        /// </summary>
        public Workspace Workspace => _workspace;

        /// <summary>
        ///     Get static SyntaxGenerator
        /// </summary>
        public SyntaxGenerator Generator =>
            _generator ?? (_generator = SyntaxGenerator.GetGenerator(_workspace, LanguageNames.CSharp));


        private string GetSourceText()
        {
            var root = _root.NormalizeWhitespace();
            Workspace.Options.WithChangedOption(CSharpFormattingOptions.IndentBraces, true);
            var formattedCode = Formatter.Format(root, Workspace);
            return formattedCode.ToFullString();
        }

        /// <summary>
        ///     Add a new subscriber and call OnCompilationTreeUpdate
        /// </summary>
        /// <param name="sub">Subscriber</param>
        public void AddSubscriber(ICompilationTreeSubscriber sub)
        {
            _subscribers.Add(sub);
            sub.OnCompilationTreeUpdate(this);
        }

        /// <summary>
        ///     Remove an existing subscriber
        /// </summary>
        /// <param name="sub">Subscriber</param>
        public void RemoveSubscriber(ICompilationTreeSubscriber sub) => _subscribers.Remove(sub);

        /// <summary>
        ///     Remove existing references and add new from ReferencesPath
        /// </summary>
        /// <param name="referencesPath">ReferencesPath</param>
        public void UpdateReferences(string referencesPath = "references/")
        {
            var compilation = _compilation
                .RemoveAllReferences()
                .AddReferences(Directory.GetFiles(referencesPath)
                    .Select(path =>
                        MetadataReference.CreateFromFile(Path.Combine(Directory.GetCurrentDirectory(), path)))
                    .ToList());

            UpdateCompilation(compilation);
        }

        /// <summary>
        ///     Create a new compilation with new SourceText
        /// </summary>
        /// <param name="sourceText">Source Text</param>
        private void UpdateSourceText(string sourceText)
        {
            UpdateTree(CSharpSyntaxTree.ParseText(sourceText), true);
        }

        /// <summary>
        ///     Update Compilation with all children (Tree, Root, ...)
        /// </summary>
        /// <param name="compilation">CSharpCompilation</param>
        private void UpdateCompilation(CSharpCompilation compilation)
        {
            _compilation = compilation;
            if (compilation.SyntaxTrees.Length > 0)
            {
                UpdateTree(compilation.SyntaxTrees[0], false);
            }

            FireUpdateOnSubscribers();
        }

        /// <summary>
        ///     Update Tree with all children (Root, ...)
        /// </summary>
        /// <param name="tree">Tree</param>
        /// <param name="updateCompilation">Update Tree with all children and Compilation</param>
        private void UpdateTree(SyntaxTree tree, bool updateCompilation)
        {
            if (updateCompilation)
            {
                UpdateCompilation(_compilation.RemoveAllSyntaxTrees().AddSyntaxTrees(tree));
            }
            else
            {
                _tree = tree;
                UpdateRoot(Tree.GetRoot(), false);
                SemanticModel = _compilation.GetSemanticModel(tree);
            }
        }

        /// <summary>
        ///     Update Root with all children
        /// </summary>
        /// <param name="node">SyntaxNode</param>
        /// <param name="updateTree">Update Root with all children an parents (Tree, Compilation)</param>
        private void UpdateRoot(SyntaxNode node, bool updateTree)
        {
            if (updateTree)
            {
                UpdateTree(_tree.WithRootAndOptions(node, CSharpParseOptions.Default), true);
            }
            else
            {
                _root = node;
            }
        }

        /// <summary>
        ///     Call OnCompilationTreeUpdate on all subscribers
        /// </summary>
        private void FireUpdateOnSubscribers()
        {
            for (var i = 0; i < _subscribers.Count; i++)
            {
                _subscribers[i].OnCompilationTreeUpdate(this);
            }
        }
    }
}