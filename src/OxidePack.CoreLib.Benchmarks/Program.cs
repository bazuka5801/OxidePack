using System;
using BenchmarkDotNet.Running;
using OxidePack.CoreLib.TestApp.Benchmarks;

namespace OxidePack.CoreLib.Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<ActionCalling>();
        }
    }
}