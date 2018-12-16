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
            // ここの i.GetType() は毎度 i.GetType() を呼ぶ方が高速。
            // var t = i.GetType() と変数に受けちゃうと Type 型のインスタンスが作られる。
            // 直接 typeof との比較をする場合、単なる仮想メソッド テーブル ポインターの比較になるっぽい。
            if (i.GetType() == typeof(A1)) Unbox<A1>(i).M();
            else if (i.GetType() == typeof(A2)) Unbox<A2>(i).M();
            else if (i.GetType() == typeof(A3)) Unbox<A3>(i).M();
            else if (i.GetType() == typeof(A4)) Unbox<A4>(i).M();
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
            if (i.GetType() == typeof(B1)) Unbox<B1>(i).M();
            else if (i.GetType() == typeof(B2)) Unbox<B2>(i).M();
            else if (i.GetType() == typeof(B3)) Unbox<B3>(i).M();
            else if (i.GetType() == typeof(B4)) Unbox<B4>(i).M();
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

    // キャストのコスト避けるために邪悪な手段を使って型判定したり unbox したり。

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ref T Unbox<T>(object obj) => ref Unsafe.As<byte, T>(ref Unsafe.As<object, PinningHelper>(ref obj).data);

    class PinningHelper { public byte data; }

    #endregion
}
