using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Jobs;
using BenchmarkDotNet.Engines;
using System;

namespace InlinedOrNot
{
    [SimpleJob(RunStrategy.Throughput)]
    public class BenchmarkCode
    {
        static int[] testData;

        static BenchmarkCode()
        {
            var r = new Random();
            testData = new int[100];

            for (int i = 0; i < testData.Length; i++)
            {
                testData[i] = r.Next();
            }
        }

        [Benchmark]
        public static int ManuallyInlined()
        {
            var sum = 0;
            foreach (var x in testData) sum = sum + x;
            return sum;
        }

        [Benchmark]
        public static int Inlining()
        {
            var sum = 0;
            foreach (var x in testData) sum = Target.Inlining(sum, x);
            return sum;
        }

        [Benchmark]
        public static int NoInlining()
        {
            var sum = 0;
            foreach (var x in testData) sum = Target.NoInlining(sum, x);
            return sum;
        }
    }
}
