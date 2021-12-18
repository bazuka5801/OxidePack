using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Formatting;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Rename;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace OxidePack.CoreLib
{
    internal class TokensEncryptor : CSharpSyntaxRewriter
    {
        private readonly IdentifierGenerator _identifierGenerator;
        private readonly EncryptorOptions _options;
        private readonly AdhocWorkspace _workspace;
        private SemanticModel _semanticModel;

        public TokensEncryptor(AdhocWorkspace workspace, EncryptorOptions options = null,
            bool visitIntoStructuredTrivia = true) : base(visitIntoStructuredTrivia)
        {
            _options = options ?? new EncryptorOptions();
            _identifierGenerator = new IdentifierGenerator();
            _workspace = workspace;

            _workspace.Options = _workspace.Options.WithChangedOption(RenameOptions.RenameOverloads, true);
            //Add cause MSBuild does not copy CSharp.Workspace.dll
            var _ = typeof(CSharpFormattingOptions);
        }

        public AdhocWorkspace MinifyIdentifiers()
        {
            foreach (var project in _workspace.CurrentSolution.Projects)
            {
                foreach (var document in project.Documents.ToList())
                {
                    _semanticModel = document.GetSemanticModelAsync().Result;
                    foreach (var diagnostic in _semanticModel.Compilation.GetDiagnostics())
                    {
                        if (diagnostic.Severity == DiagnosticSeverity.Error)
                        {
                            Console.WriteLine(diagnostic.ToString());
                        }
                    }

                    var node = _semanticModel.SyntaxTree.GetRoot();
                    node = Visit(node);


                    node = node.ReplaceTrivia(node.DescendantTrivia(), ReplaceCommentsAndRegions);
                    var (newDoc, root) = RenameAll(document.WithSyntaxRoot(node));
                    if (newDoc != null)
                    {
                        project.RemoveDocument(document.Id);
                        project.AddDocument("Plugin", root);
                    }
                }
            }

            return _workspace;
        }

        private (Document doc, SyntaxNode node) RenameAll(Document document)
        {
            _semanticModel = document.GetSemanticModelAsync().Result;
            var newNode = _semanticModel.SyntaxTree.GetRoot();
            try
            {
                foreach (var variable in _identifierGenerator.RenamedVariables)
                {
                    var nodeToSearch = newNode.DescendantNodes()
                        .OfType<VariableDeclaratorSyntax>().FirstOrDefault(x =>
                            x.Identifier.ValueText.Equals(variable.Key.Identifier.ValueText) &&
                            x.SequenceToRoot(variable.Key));
                    if (nodeToSearch == null)
                    {
                        continue;
                    }

                    (newNode, document) = Rename(nodeToSearch, document, variable.Value);
                }

                foreach (var method in _identifierGenerator.RenamedMethods)
                {
                    var nodeToSearch = newNode.DescendantNodes()
                        .OfType<MethodDeclarationSyntax>().FirstOrDefault(x =>
                            x.Identifier.ValueText.Equals(method.Key.Identifier.ValueText) &&
                            x.SequenceToRoot(method.Key));
                    if (nodeToSearch == null)
                    {
                        continue;
                    }

                    (newNode, document) = Rename(nodeToSearch, document, method.Value);
                }

                foreach (var classToRename in _identifierGenerator.RenamedTypes)
                {
                    var nodeToSearch = newNode.DescendantNodes()
                        .OfType<ClassDeclarationSyntax>().FirstOrDefault(x =>
                            x.Identifier.ValueText.Equals(classToRename.Key.Identifier.ValueText) &&
                            x.SequenceToRoot(classToRename.Key));
                    if (nodeToSearch == null)
                    {
                        continue;
                    }

                    (newNode, document) = Rename(nodeToSearch, document, classToRename.Value);
                }

                foreach (var enumToRename in _identifierGenerator.RenamedEnums)
                {
                    var nodeToSearch = newNode.DescendantNodes()
                        .OfType<EnumDeclarationSyntax>().FirstOrDefault(x =>
                            x.Identifier.ValueText.Equals(enumToRename.Key.Identifier.ValueText) &&
                            x.SequenceToRoot(enumToRename.Key));
                    if (nodeToSearch == null)
                    {
                        continue;
                    }

                    (newNode, document) = Rename(nodeToSearch, document, enumToRename.Value);
                }

                foreach (var enumMemberToRename in _identifierGenerator.RenamedEnumMembers)
                {
                    var nodeToSearch = newNode.DescendantNodes()
                        .OfType<EnumMemberDeclarationSyntax>().FirstOrDefault(x =>
                            x.Identifier.ValueText.Equals(enumMemberToRename.Key.Identifier.ValueText) &&
                            x.SequenceToRoot(enumMemberToRename.Key));
                    if (nodeToSearch == null)
                    {
                        continue;
                    }

                    (newNode, document) = Rename(nodeToSearch, document, enumMemberToRename.Value);
                }

                foreach (var parameterToRename in _identifierGenerator.RenamedParameters)
                {
                    var nodeToSearch = newNode.DescendantNodes()
                        .OfType<ParameterSyntax>().FirstOrDefault(x =>
                            x.Identifier.ValueText.Equals(parameterToRename.Key.Identifier.ValueText) &&
                            x.SequenceToRoot(parameterToRename.Key));
                    if (nodeToSearch == null)
                    {
                        continue;
                    }

                    (newNode, document) = Rename(nodeToSearch, document, parameterToRename.Value);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return (document, newNode);
        }

        private (SyntaxNode node, Document document) Rename(SyntaxNode nodeToRename, Document document, string newName)
        {
//            var symbolInfo = _semanticModel.GetSymbolInfo(nodeToRename).Symbol;
//            if (symbolInfo == null)
//            {
            var symbolInfo = _semanticModel.GetDeclaredSymbol(nodeToRename);
//                Console.WriteLine($"I can't use semantic model for "+nodeToRename);
//            }
            var solution = Renamer.RenameSymbolAsync(document.Project.Solution, symbolInfo, newName,
                _workspace.Options).Result;
            _workspace.TryApplyChanges(solution);
            document = _workspace.CurrentSolution.GetDocument(document.Id);
            _semanticModel = document.GetSemanticModelAsync().Result;
            return (_semanticModel.SyntaxTree.GetRoot(), document);
        }

        public override SyntaxNode VisitLocalDeclarationStatement(LocalDeclarationStatementSyntax node)
        {
            if (_options.LocalVarsCompressing && node.Parent.IsKind(SyntaxKind.Block))
            {
                foreach (var dec in node.Declaration.Variables)
                {
                    _identifierGenerator.GetNextName(dec);
                }
            }

            return base.VisitLocalDeclarationStatement(node);
        }

        public override SyntaxNode VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            if (_options.TypesCompressing && (node.BaseList?.Types.All(p => p.ToString() != "RustPlugin") ?? true))
            {
                _identifierGenerator.GetNextName(node);
            }

            return base.VisitClassDeclaration(node);
        }

        public override SyntaxNode VisitFieldDeclaration(FieldDeclarationSyntax node)
        {
            if (_options.FieldsCompressing)
            {
                _identifierGenerator.GetNextName(node.Declaration.Variables.First());
            }

            return base.VisitFieldDeclaration(node);
        }

        public override SyntaxNode VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            if (_options.MethodsCompressing && !node.Modifiers.Any(SyntaxKind.OverrideKeyword))
            {
                _identifierGenerator.GetNextName(node);
            }

            return base.VisitMethodDeclaration(node);
        }

        public override SyntaxNode VisitEnumDeclaration(EnumDeclarationSyntax node)
        {
            _identifierGenerator.GetNextName(node);
            return base.VisitEnumDeclaration(node);
        }

        public override SyntaxNode VisitEnumMemberDeclaration(EnumMemberDeclarationSyntax node)
        {
            _identifierGenerator.GetNextName(node);
            return base.VisitEnumMemberDeclaration(node);
        }

        public override SyntaxNode VisitParameter(ParameterSyntax node)
        {
            _identifierGenerator.GetNextName(node);
            return base.VisitParameter(node);
        }

        public override SyntaxNode VisitArgument(ArgumentSyntax node)
        {
            if (_options.LocalVarsCompressing)
            {
                _identifierGenerator.GetNextName(node);
            }

            return base.VisitArgument(node);
        }

        public override SyntaxNode VisitPropertyDeclaration(PropertyDeclarationSyntax node) =>
            base.VisitPropertyDeclaration(node);

        public override SyntaxTrivia VisitTrivia(SyntaxTrivia trivia) => base.VisitTrivia(trivia);

        public SyntaxTrivia ReplaceCommentsAndRegions(SyntaxTrivia arg1, SyntaxTrivia arg2)
        {
            if (_options.TrashRemoving)
            {
                if (arg1.IsKind(SyntaxKind.SingleLineCommentTrivia) || arg1.IsKind(SyntaxKind.MultiLineCommentTrivia)
                                                                    || arg1.IsKind(SyntaxKind.RegionDirectiveTrivia) ||
                                                                    arg1.IsKind(SyntaxKind.EndRegionDirectiveTrivia)
                                                                    || arg1.IsKind(SyntaxKind
                                                                        .DocumentationCommentExteriorTrivia)
                                                                    || arg1.IsKind(SyntaxKind
                                                                        .SingleLineDocumentationCommentTrivia))
                {
                    arg2 = SyntaxTrivia(SyntaxKind.WhitespaceTrivia, " ");
                }
                else
                {
                    arg2 = arg1;
                }
            }

            if (_options.SpacesRemoving)
            {
                if (arg1.IsKind(SyntaxKind.EndOfLineTrivia))
                {
                    arg2 = SyntaxTrivia(SyntaxKind.WhitespaceTrivia, "");
                }
                else if (arg1.IsKind(SyntaxKind.WhitespaceTrivia))
                {
                    arg2 = SyntaxTrivia(SyntaxKind.WhitespaceTrivia, " ");
                }
            }

            return arg2;
        }
    }
}