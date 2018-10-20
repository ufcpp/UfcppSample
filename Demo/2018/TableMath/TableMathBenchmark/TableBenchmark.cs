using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;
using System.Linq;
using TableMath;

namespace TableMathBenchmark
{
    /// <summary>
    /// 大まかな結果、手元のPCで、
    ///
    /// Math.Sin/Cos … 20μs
    /// MathF.Sin/Cos … 11μs
    /// SinCosTable.Sin/Cos … 3.5μs
    /// SinCosTableF.Sin/Cos … 2.5μs
    ///
    /// くらい。
    /// unsafe にするのは全く効果なし。
    /// </summary>
    public class TableBenchmark
    {
        private float[] _data;

        [GlobalSetup]
        public void Setup()
        {
            var r = new Random(1);
            _data = Enumerable.Range(0, 1000).Select(_ => (float)r.NextDouble() * 1000).ToArray();
        }

        [Benchmark] public void MathSin() { foreach (var x in _data) Math.Sin(x); }
        [Benchmark] public void MathCos() { foreach (var x in _data) Math.Cos(x); }
        [Benchmark] public void TableSin() { foreach (var x in _data) SinCosTable.Sin(x); }
        [Benchmark] public void TableCos() { foreach (var x in _data) SinCosTable.Cos(x); }
        [Benchmark] public void TableSinCos() { foreach (var x in _data) SinCosTable.SinCos(x); }
        [Benchmark] public void MathFSin() { foreach (var x in _data) MathF.Sin(x); }
        [Benchmark] public void MathFCos() { foreach (var x in _data) MathF.Cos(x); }
        [Benchmark] public void TableFSin() { foreach (var x in _data) SinCosTableF.Sin(x); }
        [Benchmark] public void TableFCos() { foreach (var x in _data) SinCosTableF.Cos(x); }
        [Benchmark] public void TableFSinCos() { foreach (var x in _data) SinCosTableF.SinCos(x); }
        [Benchmark] public void TableFSinCosOut() { foreach (var x in _data) SinCosTableF.SinCos(x, out _, out _); }
        [Benchmark] public void UnsafeTableFSin() { foreach (var x in _data) SinCosTableUnsafeF.Sin(x); }
        [Benchmark] public void UnsafeTableFCos() { foreach (var x in _data) SinCosTableUnsafeF.Cos(x); }
        [Benchmark] public void UnsafeTableFSinCos() { foreach (var x in _data) SinCosTableUnsafeF.SinCos(x); }
    }
}
