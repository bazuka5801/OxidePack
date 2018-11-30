using System.CodeDom.Compiler;
using System.Diagnostics;
using Microsoft.CodeDom.Providers.DotNetCompilerPlatform;
using System.IO;

namespace OxidePack.CoreLib.Utils
{
    public class CompileUtils
    {
        public static bool CanCompile(string program)
        {
            return !Compile(program).Errors.HasErrors;
        }
        
        public static CompilerResults Compile(string program)
        {
            CompilerResults compilerResults = null;
            using (CSharpCodeProvider provider = new CSharpCodeProvider())
            {
                var references = Directory.Exists("references") ?
                    Directory.GetFiles("references", "*.dll") : new string[0];
                compilerResults = provider.CompileAssemblyFromSource(
                    new CompilerParameters(references),
                    new string[] { program });
            }
            return compilerResults;
        }
    }
}