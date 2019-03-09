using System;
using BenchmarkDotNet.Attributes;

namespace OxidePack.CoreLib.TestApp.Benchmarks
{
    public class ActionCalling
    {
        [Benchmark(Description = "Direct")]
        public void TestDirect()
        {
            for (int i = 0; i < 100_000; i++)
            {
                Test(i, i + 1);
            }
        }
        
        [Benchmark(Description = "Action")]
        public void TestAction()
        {
            for (int i = 0; i < 100_000; i++)
            {
                action(i, i + 1);
            }
        }

        [Benchmark(Description = "Delegate")]
        public void TestDelegate()
        {
            for (int i = 0; i < 100_000; i++)
            {
                delegat(i, i + 1);
            }
        }

        public int Test(int a, int b)
        {
            return a + b;
        }
        
        
        Func<int, int, int> action;
        delegate int HandlerEmptyMethod(int a, int b);
        HandlerEmptyMethod delegat;

        public ActionCalling()
        {
            action = Test;
            delegat = Test;
        }
    }
}