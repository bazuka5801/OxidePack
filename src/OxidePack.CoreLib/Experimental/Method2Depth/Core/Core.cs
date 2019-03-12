using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using OxidePack.CoreLib.Method2Depth.Method2Sequence.Old;
using OxidePack.CoreLib.Method2Depth;

namespace OxidePack.CoreLib.Method2Depth
{
    using static SyntaxFactory;
    
    internal partial class Core : ICompilationTreeSubscriber
    {
        private CompilationTree _compilationTree;
        private ClassDeclarationSyntax _mainClass;
        private string _mainClassFullPath;
        
        public Core(CompilationTree compilationTree)
        {
            _compilationTree = compilationTree;
            compilationTree.AddSubscriber(this);
            
            // Main class was set in OnCompilationTreeUpdate
            _mainClassFullPath = _mainClass.FullPath();
        }
        
        public void Process()
        {
            // Find all local variables
            var localsResult = new LocalsVisitor().Walk(_mainClass, _compilationTree.SemanticModel);
            
            // Find all nodes that need this or _this after move in fake class
            var thisInfo = new ThisVisitor().Walk(_mainClass, _mainClassFullPath, _compilationTree.SemanticModel);
            
            // Edit tree to add this and _this - result bad compilation
            _compilationTree.Root = new ThisRewriter().Rewrite(_compilationTree.Root, thisInfo);

            var nullReturnValue = CreateNullReturnValue();
            
            var methodResults = new MethodsVisitor().Walk(_mainClass);
            
        }

        private void GenerateMethodClasses(MethodsVisitorResults methods, LocalsVisitorResults locals, ThisVisitorResults thisResults, string nullStructName)
        {
            var members = _mainClass.Members;
            foreach (var method in methods.Methods)
            {
                var mClass = new MethodClassGeneration(methods, locals, thisResults, nullStructName, );
            }
        }

        public void OnCompilationTreeUpdate(CompilationTree compilationTree)
        {
            _mainClass = compilationTree.Root.FirstInDepth<ClassDeclarationSyntax>();
        }
    }
}