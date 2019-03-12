using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace OxidePack.CoreLib.Method2Depth
{
    internal partial class Core : ICompilationTreeSubscriber
    {
        private readonly CompilationTree _compilationTree;
        private ClassDeclarationSyntax _mainClass;
        private readonly string _mainClassFullPath;
        private readonly List<MemberDeclarationSyntax> _members;

        public Core(CompilationTree compilationTree)
        {
            _compilationTree = compilationTree;
            compilationTree.AddSubscriber(this);

            // Main class was set in OnCompilationTreeUpdate
            _mainClassFullPath = _mainClass.FullPath();

            _members = new List<MemberDeclarationSyntax>();
        }

        /// <summary>
        ///     Called when CompilationTree was changed
        /// </summary>
        /// <param name="compilationTree">CompilationTree</param>
        void ICompilationTreeSubscriber.OnCompilationTreeUpdate(CompilationTree compilationTree)
        {
            _mainClass = compilationTree.Root.FirstInDepth<ClassDeclarationSyntax>();
        }

        public void Process()
        {
            // Collect information about methods
            var methodResults = new MethodsVisitor().Walk(_mainClass);

            // Find all local variables
            var localsResult = new LocalsVisitor().Walk(_mainClass, methodResults, _compilationTree.SemanticModel);

            // Edit tree to add this and _this - result bad compilation
            _compilationTree.Root = new LocalsRewriter().Rewrite(_compilationTree.Root, methodResults);

            // Find all nodes that need this or _this after move in fake class
            var thisInfo = new ThisVisitor().Walk(_mainClass, _mainClassFullPath, methodResults,
                _compilationTree.SemanticModel);

            // Edit tree to add this and _this - result bad compilation
            _compilationTree.Root = new ThisRewriter().Rewrite(_compilationTree.Root, thisInfo);

            // Edit tree to change accessibility all fields to public
            _compilationTree.Root = new FieldsRewritrer().Rewrite(_compilationTree.Root, _compilationTree.Generator);

            // Collect information about methods
            methodResults = new MethodsVisitor().Walk(_mainClass);

            // Crate a null struct to know when return is null or continue
            var nullReturnValue = CreateNullReturnValue();
            _members.Add(nullReturnValue);

            // Rewrite methods to call method body from generated classes
            _compilationTree.Root =
                new MethodsRewriter().Rewrite(_compilationTree.Root, methodResults, _compilationTree.Generator);

            // Generate classes for method based on collected information
            GenerateMethodClasses(methodResults, localsResult, thisInfo, nullReturnValue.Identifier.Text);

            // Generate pool members for MethodClasses based on collected information
            GeneratePoolMembers(methodResults, thisInfo);

            // Add new members into out class
            UpdateMainClass(_mainClass.AddMembers(_members.ToArray()));
        }

        /// <summary>
        ///     Generate the MethodClasses for methods
        /// </summary>
        /// <param name="methods">Methods Information</param>
        /// <param name="locals">Locals Information</param>
        /// <param name="thisResults">Information about this and _this</param>
        /// <param name="nullStructName">Name of NullStruct</param>
        private void GenerateMethodClasses(MethodsVisitorResults methods, LocalsVisitorResults locals,
            ThisVisitorResults thisResults, string nullStructName)
        {
            foreach (var method in methods.Methods)
            {
                var mClass = new MethodClassGeneration(method.Value, locals, thisResults, nullStructName,
                    _compilationTree.Generator).Generate();
                _members.Add(mClass);
            }
        }

        /// <summary>
        ///     Generate the pool members
        /// </summary>
        /// <param name="methods">Methods Information</param>
        /// <param name="thisResults">Information about this and _this</param>
        private void GeneratePoolMembers(MethodsVisitorResults methods, ThisVisitorResults thisResults)
        {
            foreach (var method in methods.Methods)
            {
                new PoolMembersGeneration(_members, method.Value, thisResults, _compilationTree.Generator).Generate();
            }
        }

        /// <summary>
        ///     Update MainClass and Root after changes
        /// </summary>
        /// <param name="mainClass">MainClass</param>
        private void UpdateMainClass(ClassDeclarationSyntax mainClass)
        {
            _compilationTree.Root = _compilationTree.Root.ReplaceNode(_mainClass, mainClass);
        }
    }
}