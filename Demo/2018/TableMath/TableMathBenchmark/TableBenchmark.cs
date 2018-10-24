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
    /// Math.Sin/Cos … 11μs
    /// Math.Atan2 … 20μs
    /// MathF.Sin/Cos … 11μs
    /// MathF.Atan2 … 15μs
    ///
    /// SinCosTable.Sin/Cos … 3μs
    /// SinCosTableF.Sin/Cos … 2μs
    /// SinCosTable/SinCosTableF.Atan2 … 5μs
    ///
    /// くらい。
    /// 精度2桁程度になるわりに、4倍程度の高速化と言うのを速いとみるかどうか…
    ///
    /// unsafe にするのはそんなに効果なし(せいぜい、2.2μs → 2.0μs)。
    ///
    /// ## タプル戻り値
    /// あと、(T sin, T cos) SinCos(T) が、なぜか float 版の時に遅い。
    /// (double 版だとタプル (double sin, double cos) で返しても十分速いんだけど。)
    /// タプル構築の最適化が double の方が掛かりやすいっぽい？なので、out 引数で返すバージョンを用意してる。
    /// out 引数の方は想定通りの速度(Sin, Cos 単体と遜色ない速度)出てた。
    /// </summary>
    public class TableBenchmark
    {
        private float[] _longData;
        private float[] _shortData;

        [GlobalSetup]
        public void Setup()
        {
            var r = new Random(1);
            _longData = Enumerable.Range(0, 1000).Select(_ => (float)r.NextDouble() * 1000 - 500).ToArray();
            _shortData = Enumerable.Range(0, 30).Select(_ => (float)r.NextDouble() * 1000 - 500).ToArray();
        }

        // 和を取って返してるのはインライン化で消えないように
        [Benchmark] public double MathSin() { var sum = 0.0; foreach (var x in _longData) sum += Math.Sin(x); return sum; }
        [Benchmark] public double MathCos() { var sum = 0.0; foreach (var x in _longData) sum += Math.Cos(x); return sum; }
        [Benchmark] public double TableSin() { var sum = 0.0; foreach (var x in _longData) sum += SinCosTable.Sin(x); return sum; }
        [Benchmark] public double TableCos() { var sum = 0.0; foreach (var x in _longData) sum += SinCosTable.Cos(x); return sum; }
        [Benchmark] public double TableSinCos() { var sum = 0.0; foreach (var x in _longData) { var (s, c) = SinCosTable.SinCos(x); sum += s; } return sum; }
        [Benchmark] public float MathFSin() { var sum = 0.0f; foreach (var x in _longData) sum += MathF.Sin(x); return sum; }
        [Benchmark] public float MathFCos() { var sum = 0.0f; foreach (var x in _longData) sum += MathF.Cos(x); return sum; }
        [Benchmark] public float TableFSin() { var sum = 0.0f; foreach (var x in _longData) sum += SinCosTableF.Sin(x); return sum; }
        [Benchmark] public float TableFCos() { var sum = 0.0f; foreach (var x in _longData) sum += SinCosTableF.Cos(x); return sum; }
        [Benchmark] public float TableFSinCos() { var sum = 0.0f; foreach (var x in _longData) { var (s, c) = SinCosTableF.SinCos(x); sum += s; } return sum; }
        [Benchmark] public float TableFSinCosOut() { var sum = 0.0f; foreach (var x in _longData) { SinCosTableF.SinCos(x, out var s, out _); sum += s; } return sum; }
        [Benchmark] public float UnsafeTableFSin() { var sum = 0.0f; foreach (var x in _longData) sum += SinCosTableUnsafeF.Sin(x); return sum; }
        [Benchmark] public float UnsafeTableFCos() { var sum = 0.0f; foreach (var x in _longData) sum += SinCosTableUnsafeF.Cos(x); return sum; }
        [Benchmark] public float UnsafeTableFSinCos() { var sum = 0.0f; foreach (var x in _longData) { var (s, c) = SinCosTableUnsafeF.SinCos(x); sum += s; } return sum; }
        [Benchmark] public double MathAtan2() { var sum = 0.0; foreach (var x in _shortData) foreach (var y in _shortData) sum += Math.Atan2(x, y); return sum; }
        [Benchmark] public float MathFAtan2() { var sum = 0.0f; foreach (var x in _shortData) foreach (var y in _shortData) sum += MathF.Atan2(x, y); return sum; }
        [Benchmark] public double TableAtan2() { var sum = 0.0; foreach (var x in _shortData) foreach (var y in _shortData) sum += SinCosTable.Atan2(x, y); return sum; }
        [Benchmark] public float TableFAtan2() { var sum = 0.0f; foreach (var x in _shortData) foreach (var y in _shortData) sum += SinCosTableF.Atan2(x, y); return sum; }
    }
}
