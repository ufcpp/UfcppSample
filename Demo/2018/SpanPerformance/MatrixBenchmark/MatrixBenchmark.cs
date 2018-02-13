using BenchmarkDotNet.Attributes;
using System;

public class MatrixBenchmark
{
    const int Loops = 100;

    static readonly float[] _a;
    static readonly float[] _b;
    static readonly float[] _id = new float[] { 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1 };

    static MatrixBenchmark()
    {
        var r = new Random(1);

        _a = RandomMatrix(r);
        _b = RandomMatrix(r);
    }

    private static float[] RandomMatrix(Random r)
    {
        var d = new double[16];
        for (int i = 0; i < 16; i++)
        {
            d[i] = r.NextDouble();
        }

        for (int i = 0; i < 4; i++)
        {
            var len = Math.Sqrt(d[i + 0] * d[i + 0] + d[i + 1] * d[i + 1] + d[i + 2] * d[i + 2] + d[i + 3] * d[i + 3]);
            d[i + 0] /= len;
            d[i + 1] /= len;
            d[i + 2] /= len;
            d[i + 3] /= len;
        }

        var a = new float[16];
        for (int i = 0; i < 16; i++) a[i] = (float)d[i];
        return a;
    }

    private static ByValue.Matrix GetByValMatrix(float[] a) => new ByValue.Matrix(a[0], a[1], a[2], a[3], a[4], a[5], a[6], a[7], a[8], a[9], a[10], a[11], a[12], a[13], a[14], a[15]);
    private static ByRef.Matrix GetByRefMatrix(float[] a) => new ByRef.Matrix(a[0], a[1], a[2], a[3], a[4], a[5], a[6], a[7], a[8], a[9], a[10], a[11], a[12], a[13], a[14], a[15]);
    private static Simd.Matrix GetSimdMatrix(float[] a) => new Simd.Matrix(a[0], a[1], a[2], a[3], a[4], a[5], a[6], a[7], a[8], a[9], a[10], a[11], a[12], a[13], a[14], a[15]);

    [Benchmark]
    public ByValue.Matrix ByValue()
    {
        var a = GetByValMatrix(_a);
        var b = GetByValMatrix(_b);
        var m = GetByValMatrix(_id);
        for (int i = 1; i < Loops; i++) m = a * m + b;
        return m;
    }

    [Benchmark]
    public ByRef.Matrix ByRef()
    {
        var a = GetByRefMatrix(_a);
        var b = GetByRefMatrix(_b);
        var m = GetByRefMatrix(_id);
        for (int i = 1; i < Loops; i++) m = a * m + b;
        return m;
    }

    [Benchmark]
    public Simd.Matrix Simd()
    {
        var a = GetSimdMatrix(_a);
        var b = GetSimdMatrix(_b);
        var m = GetSimdMatrix(_id);
        for (int i = 1; i < Loops; i++) m = a * m + b;
        return m;
    }
}
