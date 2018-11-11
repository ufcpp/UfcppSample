using BenchmarkDotNet.Attributes;
using System;
using System.Runtime.CompilerServices;

enum A1 : sbyte { X = 1, Y = 2, Z = 4 }
enum A2 : byte { X = 1, Y = 2, Z = 4 }
enum B1 : short { X = 1, Y = 2, Z = 4 }
enum B2 : ushort { X = 1, Y = 2, Z = 4 }
enum C1 : int { X = 1, Y = 2, Z = 4 }
enum C2 : uint { X = 1, Y = 2, Z = 4 }
enum D1 : long { X = 1, Y = 2, Z = 4 }
enum D2 : ulong { X = 1, Y = 2, Z = 4 }

public class EnumHasFlagBenchmark
{
    /// <summary>
    /// enum をジェネリックに HasFlag をするために、Unsafe で無理やり整数型に変換して AND 取る関数。
    /// </summary>
    /// <remarks>
    /// sizeof は JIT 時定数になっているので、所望の if 以外のところは完全に消えてくれる。
    /// なので、ほぼ、非ジェネリックな実装と同程度のコストになってるはず。
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static bool UnsafeHasFlag<T>(T x, T y)
        where T : unmanaged, Enum
    {
        if (Unsafe.SizeOf<T>() == 1) return (Unsafe.As<T, byte>(ref x) & Unsafe.As<T, byte>(ref y)) != 0;
        else if (Unsafe.SizeOf<T>() == 2) return (Unsafe.As<T, ushort>(ref x) & Unsafe.As<T, ushort>(ref y)) != 0;
        else if (Unsafe.SizeOf<T>() == 4) return (Unsafe.As<T, uint>(ref x) & Unsafe.As<T, uint>(ref y)) != 0;
        else if (Unsafe.SizeOf<T>() == 8) return (Unsafe.As<T, ulong>(ref x) & Unsafe.As<T, ulong>(ref y)) != 0;
        else { Throw(); return default; }
    }

    private static void Throw() => throw new InvalidOperationException();

    /// <summary>
    /// 比較。
    /// <see cref="Enum.HasFlag(Enum)"/> を呼ぶだけ。
    /// </summary>
    /// <remarks>
    /// .NET Core 2.1 ではランタイム上の最適化で単なる AND に置き換わる。
    /// それ以前のランタイムを使うと box 化を起こしてすさまじく遅い。
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static bool EnumHasFlag<T>(T x, T y)
        where T : unmanaged, Enum
        => x.HasFlag(y);

    // 比較。以下、非ジェネリック実装を全部の型に用意。
    static bool NonGenericHasFlag(A1 x, A1 y) => (((byte)x) & ((byte)y)) != 0;
    static bool NonGenericHasFlag(A2 x, A2 y) => (((byte)x) & ((byte)y)) != 0;
    static bool NonGenericHasFlag(B1 x, B1 y) => (((ushort)x) & ((ushort)y)) != 0;
    static bool NonGenericHasFlag(B2 x, B2 y) => (((ushort)x) & ((ushort)y)) != 0;
    static bool NonGenericHasFlag(C1 x, C1 y) => (((uint)x) & ((uint)y)) != 0;
    static bool NonGenericHasFlag(C2 x, C2 y) => (((uint)x) & ((uint)y)) != 0;
    static bool NonGenericHasFlag(D1 x, D1 y) => (((ulong)x) & ((ulong)y)) != 0;
    static bool NonGenericHasFlag(D2 x, D2 y) => (((ulong)x) & ((ulong)y)) != 0;

    A1[] _a1;
    A2[] _a2;
    B1[] _b1;
    B2[] _b2;
    C1[] _c1;
    C2[] _c2;
    D1[] _d1;
    D2[] _d2;

    [GlobalSetup]
    public void Setup()
    {
        _a1 = new A1[8];
        _a2 = new A2[8];
        _b1 = new B1[8];
        _b2 = new B2[8];
        _c1 = new C1[8];
        _c2 = new C2[8];
        _d1 = new D1[8];
        _d2 = new D2[8];

        for (int i = 0; i < 8; i++)
        {
            _a1[i] = (A1)i;
            _a2[i] = (A2)i;
            _b1[i] = (B1)i;
            _b2[i] = (B2)i;
            _c1[i] = (C1)i;
            _c2[i] = (C2)i;
            _d1[i] = (D1)i;
            _d2[i] = (D2)i;
        }
    }

    [Benchmark]
    public bool WithUnsafe()
    {
        bool b = false;
        foreach (var x in _a1) foreach (var y in _a1) b ^= UnsafeHasFlag(x, y);
        foreach (var x in _a2) foreach (var y in _a2) b ^= UnsafeHasFlag(x, y);
        foreach (var x in _b1) foreach (var y in _b1) b ^= UnsafeHasFlag(x, y);
        foreach (var x in _b2) foreach (var y in _b2) b ^= UnsafeHasFlag(x, y);
        foreach (var x in _c1) foreach (var y in _c1) b ^= UnsafeHasFlag(x, y);
        foreach (var x in _c2) foreach (var y in _c2) b ^= UnsafeHasFlag(x, y);
        foreach (var x in _d1) foreach (var y in _d1) b ^= UnsafeHasFlag(x, y);
        foreach (var x in _d2) foreach (var y in _d2) b ^= UnsafeHasFlag(x, y);
        return b;
    }

    [Benchmark]
    public bool WithEnum()
    {
        bool b = false;
        foreach (var x in _a1) foreach (var y in _a1) b ^= EnumHasFlag(x, y);
        foreach (var x in _a2) foreach (var y in _a2) b ^= EnumHasFlag(x, y);
        foreach (var x in _b1) foreach (var y in _b1) b ^= EnumHasFlag(x, y);
        foreach (var x in _b2) foreach (var y in _b2) b ^= EnumHasFlag(x, y);
        foreach (var x in _c1) foreach (var y in _c1) b ^= EnumHasFlag(x, y);
        foreach (var x in _c2) foreach (var y in _c2) b ^= EnumHasFlag(x, y);
        foreach (var x in _d1) foreach (var y in _d1) b ^= EnumHasFlag(x, y);
        foreach (var x in _d2) foreach (var y in _d2) b ^= EnumHasFlag(x, y);
        return b;
    }

    [Benchmark]
    public bool NonGeneric()
    {
        bool b = false;
        foreach (var x in _a1) foreach (var y in _a1) b ^= NonGenericHasFlag(x, y);
        foreach (var x in _a2) foreach (var y in _a2) b ^= NonGenericHasFlag(x, y);
        foreach (var x in _b1) foreach (var y in _b1) b ^= NonGenericHasFlag(x, y);
        foreach (var x in _b2) foreach (var y in _b2) b ^= NonGenericHasFlag(x, y);
        foreach (var x in _c1) foreach (var y in _c1) b ^= NonGenericHasFlag(x, y);
        foreach (var x in _c2) foreach (var y in _c2) b ^= NonGenericHasFlag(x, y);
        foreach (var x in _d1) foreach (var y in _d1) b ^= NonGenericHasFlag(x, y);
        foreach (var x in _d2) foreach (var y in _d2) b ^= NonGenericHasFlag(x, y);
        return b;
    }
}
