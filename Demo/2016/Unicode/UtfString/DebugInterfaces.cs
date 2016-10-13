// インターフェイス越しでもヒープ確保なしでアクセスするためにはこのくらい型引数を並べないと無理
// 使う側も IString<A, B, C, D> みたいな冗長な書き方が必要になるんで、実用性はあんまりなさげ。
// テスト用。

using System.Collections.Generic;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("UnitTestUtfString")]

namespace UtfString
{
    internal interface ICodePointEnumerable<TEnumerator>
        where TEnumerator : struct, IEnumerator<CodePoint>
    {
        TEnumerator GetEnumerator();
    }

    internal interface IIndexEnumerable<TIndex, TEnumerator>
        where TIndex : struct
        where TEnumerator : struct, IEnumerator<TIndex>
    {
        TEnumerator GetEnumerator();
    }

    internal interface IString<TIndex, TEnumerator, TIndexEnumerable, TIndexEnumerator> : ICodePointEnumerable<TEnumerator>
        where TIndex : struct
        where TEnumerator : struct, IEnumerator<CodePoint>
        where TIndexEnumerator : struct, IEnumerator<TIndex>
        where TIndexEnumerable : struct, IIndexEnumerable<TIndex, TIndexEnumerator>
    {
        CodePoint this[TIndex index] { get; }
        int Length { get; }
        TIndexEnumerable Indexes { get; }
    }
}
