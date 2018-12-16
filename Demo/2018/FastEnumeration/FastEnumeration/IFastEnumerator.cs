using System.Collections.Generic;

namespace FastEnumeration
{
    /// <summary>
    /// MoveNext/Current で2回 virtual call するのが嫌で、1つにまとめようというもの。
    ///
    /// <typeparamref name="T"/> の共変性を確保するために、戻り値が T で out の方が bool になってしまう。
    /// なので結構使い勝手は悪い。
    /// out 引数とか、タプルを使った (T, bool) で共変性を担保できないっていう .NET の問題。
    /// </summary>
    interface IFastEnumerator<out T>
    {
        T TryMoveNext(out bool success);
    }

    /// <summary>
    /// <see cref="IEnumerable{T}"/> 同様、<see cref="IFastEnumerator{T}"/> を返すだけのインターフェイスも。
    /// ただ、今回のベンチマークでは enumerator の方だけパフォーマンスを見るんで、こいつは使ってない。
    /// </summary>
    interface IFastEnumerable<out T>
    {
        IFastEnumerator<T> GetEnumerator();
    }
}
