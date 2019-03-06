using System.CodeDom.Compiler;
using System.Collections.Generic;
using Microsoft.CodeDom.Providers.DotNetCompilerPlatform;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;

namespace OxidePack.CoreLib.Utils
{
    public class CompileUtils
    {
        public static bool CanCompile(string program, string referencesFolder = "references")
        {
            return !Compile(program, referencesFolder).Errors.HasErrors;
        }
        
        public static CompilerResults Compile(string program, string referencesFolder = "references")
        {
            CompilerResults compilerResults = null;
            using (CSharpCodeProvider provider = new CSharpCodeProvider())
            {
                var references = Directory.Exists(referencesFolder) ?
                    Directory.GetFiles(referencesFolder, "*.dll").Select(p=>Path.Combine(Directory.GetCurrentDirectory(), p)).ToArray() : new string[0];
                compilerResults = provider.CompileAssemblyFromSource(
                    new CompilerParameters(references),
                    new string[] { program });
            }
            return compilerResults;
        }
        
        public static (byte[], List<Data.CompilerError>) CompileAssembly(string program, string assemblyName, string referencesFolder = "server")
        {
            byte[] compiled = null;
            List<Data.CompilerError> errors = new List<Data.CompilerError>();
            
            List<MetadataReference> references = new List<MetadataReference>
            {
//                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
//                MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location)
            };

            referencesFolder = $"references/{referencesFolder}";

            references.AddRange(
                (Directory.Exists(referencesFolder) ? Directory.GetFiles(referencesFolder, "*.dll") : new string[0])
                .Select(p => MetadataReference.CreateFromFile(p)));
            
            CSharpCompilation compilation = CSharpCompilation.Create(
                assemblyName,
                syntaxTrees: new[] { CSharpSyntaxTree.ParseText(program, new CSharpParseOptions(LanguageVersion.CSharp7_2)) },
                references: references,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
            
            using (var ms = new MemoryStream())
            {
                EmitResult result = compilation.Emit(ms);

                if (!result.Success)
                {
                    IEnumerable<Diagnostic> failures = result.Diagnostics.Where(diagnostic => 
                        diagnostic.IsWarningAsError || 
                        diagnostic.Severity == DiagnosticSeverity.Error);

                    foreach (Diagnostic diagnostic in failures)
                    {
                        var linespan = diagnostic.Location.GetLineSpan();
                        var line = linespan.Span.Start.Line + 1;
                        var column = linespan.Span.Start.Character + 1;
                        var lineEnd = linespan.Span.End.Line + 1;
                        var columnEnd = linespan.Span.End.Character + 1;
                        errors.Add(new Data.CompilerError()
                        {
                            line = line, column = column,
                            lineEnd = lineEnd, columnEnd = columnEnd,
                            errorText = diagnostic.GetMessage()
                        });
//                        errors.Add($"{diagnostic.Id} [{line}:{column}]: {diagnostic.GetMessage()}");
//                        Console.Error.WriteLine("{0}: {1}", diagnostic.Id, diagnostic.GetMessage());
                    }
                }
                else
                {
                    ms.Seek(0, SeekOrigin.Begin);
                    compiled = ms.ToArray();
                }
            }

            return (compiled, errors);
        }
    }
}