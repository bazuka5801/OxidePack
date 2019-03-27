using System.Linq;
using BenchmarkDotNet.Attributes;

namespace OxidePack.CoreLib.TestApp.Benchmarks
{
    public class TheEasiestBenchmark
    {
        [Params(2,4,8)]
        public int Depth;


        [Benchmark(Description = "Depth")]
        public void TestVoidDepth()
        {
            for (int i = 0; i < 1000; i++)
            {
                Testing(Depth, i + 1, i + 2, i + 3, i + 4);
            }
        }
        [Benchmark(Description = "While")]
        public void TestVoidWhile()
        {
            for (int i = 0; i < 1000; i++)
            {
                Testing2(Depth, i + 1, i + 2, i + 3, i + 4);
            }
        }

        void Testing(int i, int i1, int i2, int i3, int i4)
        {
            if (i-1 > 0)
                Testing(i-1, i1, i2, i3, i4);
        }

        void Testing2(int i, int i1, int i2, int i3, int i4)
        {
            while (--i > 0)
            {

            }
        }
    }
}