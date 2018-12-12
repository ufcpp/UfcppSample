using BenchmarkDotNet.Attributes;
using System;
using System.Runtime.CompilerServices;

/// <summary>
/// I i = new A1();
/// i.M();
///
/// みたいなやつの呼び出しを
/// - 素直に仮想呼び出しでやるか
/// - if (i is A) ((A)i).M(); みたいに分岐コードを書いてやるか
/// のパフォーマンスを調べる。
/// </summary>
public class CallBenchmark
{
    private I[] _dataA;
    private I[] _dataB;

    [GlobalSetup]
    public void Setup()
    {
        _dataA = A.GetData();
        _dataB = B.GetData();
    }

    /// <summary>
    /// 素直に仮想呼び出し。
    /// </summary>
    /// <remarks>
    /// </remarks>
    private void CallVirt(ReadOnlySpan<I> data)
    {
        foreach (var i in data)
        {
            i.M();
        }
    }

    /// <summary>
    /// ある程度来る型がわかっている場合、if 文を並べた方が速かったりもする。
    ///
    /// 仮想呼び出しはそこそこのコストが掛かる:
    /// 1. .NET の仮想テーブルは2段構成らしく、間接参照が2回発生
    /// 2. インライン展開が全く効かなくなる
    ///
    /// A 群の M はインライン展開を抑止してるので、1 の方のコスト除けにしかならない。
    /// それでも、if が4個程度だったら十分速くなるみたい。
    /// (ほんのちょっとの差なので、こんな最適化をやる意味はあんまりない。)
    /// </summary>
    private void BranchesA(ReadOnlySpan<I> data)
    {
        foreach (var i in data)
        {
            var t = GetObjectMethodTablePointer(i);
            if (t == A1TablePointer) Unbox<A1>(i).M();
            else if (t == A2TablePointer) Unbox<A2>(i).M();
            else if (t == A3TablePointer) Unbox<A3>(i).M();
            else if (t == A4TablePointer) Unbox<A4>(i).M();
            else i.M();
        }
    }

    /// <summary>
    /// <see cref="BranchesA"/> と同じ最適化を B 群にも。
    ///
    /// B 群の方はインライン展開が掛かるように作ってある。
    /// インライン展開の有無はほんとにパフォーマンスに大差が付くんで、
    /// やってることは A 群と同じなのに、こっちは圧倒的に速い。
    /// (M が空っぽなので、全部のコードがきれいさっぱりなくなってもいい気もするけど、そこまでの最適化は掛かってなさそう。)
    ///
    /// あと、B1 の確率が高いので、分岐予測が効きやすいというのもある。
    /// </summary>
    /// <param name="data"></param>
    private void BranchesB(ReadOnlySpan<I> data)
    {
        foreach (var i in data)
        {
            var t = GetObjectMethodTablePointer(i);
            if (t == B1TablePointer) Unbox<B1>(i).M();
            else if (t == B2TablePointer) Unbox<B2>(i).M();
            else if (t == B3TablePointer) Unbox<B3>(i).M();
            else if (t == B4TablePointer) Unbox<B4>(i).M();
            else i.M();
        }
    }

    /* ちなみに、結果の一例:
    Method |     Mean |     Error |    StdDev |
---------- |---------:|----------:|----------:|
 CallVirtA | 60.30 us | 0.6552 us | 0.6129 us |
 CallVirtB | 61.44 us | 1.1861 us | 1.1649 us |
 BranchesA | 55.22 us | 0.8354 us | 0.7815 us |
 BranchesB | 18.29 us | 0.3577 us | 0.5130 us |
     */

    [Benchmark] public void CallVirtA() => CallVirt(_dataA);
    [Benchmark] public void CallVirtB() => CallVirt(_dataB);
    [Benchmark] public void BranchesA() => BranchesA(_dataA);
    [Benchmark] public void BranchesB() => BranchesB(_dataB);

    #region 黒魔術

    // is とかキャストのコスト避けるために邪悪な手段を使って型判定したり unbox したり。

    private static readonly IntPtr A1TablePointer = GetObjectMethodTablePointer(new A1());
    private static readonly IntPtr A2TablePointer = GetObjectMethodTablePointer(new A2());
    private static readonly IntPtr A3TablePointer = GetObjectMethodTablePointer(new A3());
    private static readonly IntPtr A4TablePointer = GetObjectMethodTablePointer(new A4());
    private static readonly IntPtr B1TablePointer = GetObjectMethodTablePointer(new B1());
    private static readonly IntPtr B2TablePointer = GetObjectMethodTablePointer(new B2());
    private static readonly IntPtr B3TablePointer = GetObjectMethodTablePointer(new B3());
    private static readonly IntPtr B4TablePointer = GetObjectMethodTablePointer(new B4());

    // https://github.com/dotnet/coreclr/blob/ef93a727984dbc5b8925a0c2d723be6580d20460/src/System.Private.CoreLib/src/System/Runtime/CompilerServices/RuntimeHelpers.cs#L222)
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static IntPtr GetObjectMethodTablePointer(object obj) => Unsafe.Add(ref Unsafe.As<byte, IntPtr>(ref Unsafe.As<object, PinningHelper>(ref obj).data), -1);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ref T Unbox<T>(object obj) => ref Unsafe.As<byte, T>(ref Unsafe.As<object, PinningHelper>(ref obj).data);

    class PinningHelper { public byte data; }

    #endregion
}
