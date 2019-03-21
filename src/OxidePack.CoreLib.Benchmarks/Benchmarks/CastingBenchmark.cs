using BenchmarkDotNet.Attributes;

namespace OxidePack.CoreLib.TestApp.Benchmarks
{
    public class CastingBenchmark
    {
        public class MyClass
        {
            public int a = 0;
        }

        public object myClassObj = new MyClass();
        public MyClass myClass = new MyClass();

        public MyClass Cast()
        {
            return (MyClass) myClassObj;
        }
        public MyClass Direct()
        {
            return myClass;
        }

        [Benchmark(Description = "Direct")]
        public void TestDirect()
        {
            for (int i = 0; i < 100_000; i++)
            {
                Direct();
            }
        }

        [Benchmark(Description = "Cast")]
        public void TestCast()
        {
            for (int i = 0; i < 100_000; i++)
            {
                Cast();
            }
        }
    }
}