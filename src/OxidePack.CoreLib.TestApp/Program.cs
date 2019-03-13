using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace OxidePack.CoreLib.TestApp
{
    internal class Program
    {
        private static Random rand = new Random();

        private static void Main(string[] args)
        {
            var result = File.ReadAllText("1.cs");
            File.WriteAllText($"E:\\Work\\OxidePack\\src\\OxidePack.CoreLib.Benchmarks\\Inputs\\Source.cs", result);
            Stopwatch sw = Stopwatch.StartNew();

            for (int i = 0; i < 2; i++)
            {
//                result =  new Method2Sequence().ProcessSource(result);
                result = new Method2Depth.Method2Depth().ProcessSource(result, true);
                Console.WriteLine(i);
            }
            File.WriteAllText($"E:\\Work\\OxidePack\\src\\OxidePack.CoreLib.Benchmarks\\Inputs\\Encrypted.cs", result);

//            result = DebugLoggerRewriter.Process(result);
            File.WriteAllText("NpcSystem.cs", result);
            Console.WriteLine(sw.ElapsedMilliseconds+"ms");

        }
    }
}