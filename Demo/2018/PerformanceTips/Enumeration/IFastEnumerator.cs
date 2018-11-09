using System.Collections.Generic;

namespace Enumeration
{
    /// <summary>
    /// IEnumerator が MoveNext/Current の2メソッドに分かれてるのが嫌で、1つにまとめたインターフェイス。
    /// </summary>
    /// <remarks>
    /// .NET の covariance の制限で、以下のようなメソッドにはできない。
    ///
    /// <code><![CDATA[
    /// (bool success, T value) MoveNext(); // タプルが covariant にできない
    /// bool TryMoveNext(out T value); // out 引数が covariant にできない
    /// ]]></code>
    ///
    /// ちなみに、2メソッドに分かれているというっても、インライン展開される限りには大してコストかからない。
    /// <see cref="List{T}.Enumerator"/> みたいに具象型(特に構造体)を直接使う分には全然コストにならなくて、
    /// インターフェイス越しに呼ぶときだけが問題になる。
    /// </remarks>
    public interface IFastEnumerator<out T>
    {
        T TryMoveNext(out bool success);
    }
}
