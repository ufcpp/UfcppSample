using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using System;

namespace VirtualCall
{
    [SimpleJob(RunStrategy.Throughput)]
    public class BenchmarkCode
    {
        static A a = new A();
        static B b = new B();
        const int Loops = 100;

        [Benchmark]
        public static int DirectA()
        {
            var sum = 0;
            for (int i = 0; i < Loops; i++) sum += Target.CallA(a);
            return sum;
        }

        [Benchmark]
        public static int DirectB()
        {
            var sum = 0;
            for (int i = 0; i < Loops; i++) sum += Target.CallB(b);
            return sum;
        }

        [Benchmark]
        public static int InterfaceA()
        {
            var sum = 0;
            for (int i = 0; i < Loops; i++) sum += Target.CallInterface(a);
            return sum;
        }

        [Benchmark]
        public static int InterfaceB()
        {
            var sum = 0;
            for (int i = 0; i < Loops; i++) sum += Target.CallInterface(b);
            return sum;
        }

        [Benchmark]
        public static int GenericA()
        {
            var sum = 0;
            for (int i = 0; i < Loops; i++) sum += Target.Call(a);
            return sum;
        }

        [Benchmark]
        public static int GenericB()
        {
            var sum = 0;
            for (int i = 0; i < Loops; i++) sum += Target.Call(b);
            return sum;
        }
    }
}
