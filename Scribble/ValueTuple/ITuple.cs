using System.Collections.Generic;

namespace ValueTuples
{
    /// <summary>
    /// loosely-typed タプル。
    /// </summary>
    /// <remarks>
    /// <see cref="ValueTuple"/> は strongly-typed で、リフレクションなしでのシリアライズが結構きつい。
    /// このインターフェイスを介することで、<see cref="ValueTuple"/> を loosely に使う。
    /// object なので boxing 起きまくりなのがちょっと懸念。
    ///
    /// <![CDATA[ ValueTuple<ValueTuple<T1, T2>, ValueTuple<T3, T4>> ]]> みたいな奴は再帰的に展開する。
    /// この例だと、<see cref="Count"/> は4、<see cref="Values"/> は { T1, T2, T3, T4 } 的な1次元リストが返る。
    /// </remarks>
    public interface ITuple
    {
        /// <summary>
        /// 要素数。
        /// </summary>
        int Count { get; }

        IEnumerable<object> Values { get; }

        object this[int index] { get; set; }
    }
}
