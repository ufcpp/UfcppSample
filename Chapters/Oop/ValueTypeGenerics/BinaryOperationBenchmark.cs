using BenchmarkDotNet.Attributes;
using System;
using ValueTypeGenerics.GenericArithmeticOperators;

namespace ValueTypeGenerics
{
    public class BinaryOperationBenchmark
    {
        static int[] items = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        static int Sum = 45;
        static int Prod = 362880;

        [Benchmark]
        public static void EmmbeddedOperator()
        {
            if (GenericArithmeticOperators.Program.Sum(items) != Sum) throw new InvalidOperationException();
            if (GenericArithmeticOperators.Program.Prod(items) != Prod) throw new InvalidOperationException();
        }

        [Benchmark]
        public static void Interface()
        {
            if (GenericArithmeticOperators.Interface.Program.Sum(items, new Add()) != Sum) throw new InvalidOperationException();
            if (GenericArithmeticOperators.Interface.Program.Sum(items, new Mul()) != Prod) throw new InvalidOperationException();
        }

        [Benchmark]
        public static void Generics()
        {
            if (GenericArithmeticOperators.Generics.Program.Sum(items, new Add()) != Sum) throw new InvalidOperationException();
            if (GenericArithmeticOperators.Generics.Program.Sum(items, new Mul()) != Prod) throw new InvalidOperationException();
        }

        [Benchmark]
        public static void PseudoStatic()
        {
            if (GenericArithmeticOperators.PseudoStatic.Program.Sum<int, Add>(items) != Sum) throw new InvalidOperationException();
            if (GenericArithmeticOperators.PseudoStatic.Program.Sum<int, Mul>(items) != Prod) throw new InvalidOperationException();
        }
    }
}
