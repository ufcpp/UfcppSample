using StringLiteral;
using System;

// NonCopyableAnalyzer の機能:
S s1 = new();
S s2 = s1; // 構造体の代入(コピー)を禁止する

[NonCopyable]
struct S { }

// StringLiteralGenerator の機能:
partial class Literal
{
    [Utf8("aあ😀")]
    public static partial ReadOnlySpan<byte> M();
}
