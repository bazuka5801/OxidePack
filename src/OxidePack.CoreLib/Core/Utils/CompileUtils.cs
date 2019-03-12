using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeDom.Providers.DotNetCompilerPlatform;
using CompilerError = OxidePack.Data.CompilerError;

namespace OxidePack.CoreLib.Utils
{
    public class CompileUtils
    {
        public static bool CanCompile(string program, string referencesFolder = "references") =>
            !Compile(program, referencesFolder).Errors.HasErrors;

        public static CompilerResults Compile(string program, string referencesFolder = "references")
        {
            CompilerResults compilerResults = null;
            using (var provider = new CSharpCodeProvider())
            {
                var references = Directory.Exists(referencesFolder)
                    ? Directory.GetFiles(referencesFolder, "*.dll")
                        .Select(p => Path.Combine(Directory.GetCurrentDirectory(), p)).ToArray()
                    : new string[0];
                compilerResults = provider.CompileAssemblyFromSource(
                    new CompilerParameters(references), program);
            }

            return compilerResults;
        }

        public static (byte[], List<CompilerError>) CompileAssembly(string program, string assemblyName,
            string referencesFolder = "server")
        {
            byte[] compiled = null;
            var errors = new List<CompilerError>();

            var references = new List<MetadataReference>();

            referencesFolder = $"references/{referencesFolder}";

            references.AddRange(
                (Directory.Exists(referencesFolder) ? Directory.GetFiles(referencesFolder, "*.dll") : new string[0])
                .Select(p => MetadataReference.CreateFromFile(p)));

            var compilation = CSharpCompilation.Create(
                assemblyName,
                new[] {CSharpSyntaxTree.ParseText(program, new CSharpParseOptions(LanguageVersion.CSharp7_2))},
                references,
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            using (var ms = new MemoryStream())
            {
                var result = compilation.Emit(ms);

                if (!result.Success)
                {
                    var failures = result.Diagnostics.Where(diagnostic =>
                        diagnostic.IsWarningAsError ||
                        diagnostic.Severity == DiagnosticSeverity.Error);

                    foreach (var diagnostic in failures)
                    {
                        var linespan = diagnostic.Location.GetLineSpan();
                        var line = linespan.Span.Start.Line + 1;
                        var column = linespan.Span.Start.Character + 1;
                        var lineEnd = linespan.Span.End.Line + 1;
                        var columnEnd = linespan.Span.End.Character + 1;
                        errors.Add(new CompilerError
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