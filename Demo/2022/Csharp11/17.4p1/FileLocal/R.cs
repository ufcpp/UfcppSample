using System.Text.RegularExpressions;

namespace FileLocal;

internal partial class R
{
    // file 修飾子、Source Generator で使う需要が高い。
    // 例えば、RegexGenerator は早速使ってる。
    [GeneratedRegex(@"\d+")]
    public static partial Regex M();

    // ↑このメソッドから、
    // file sealed class M_0 : Regex { } みたいなクラスが作られてる。
}
