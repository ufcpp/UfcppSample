Console.WriteLine(m("true"));
Console.WriteLine(m("false"));
Console.WriteLine(m("other"));

// ReadOnlySpan<char> に対して is とか switch で文字列リテラルとの比較ができるようになった。
static bool? m(ReadOnlySpan<char> s) => s switch
{
    "true" => true,
    "false" => false,
    _ => null,
};

#if false
// ちなみに、UTF-8 リテラルを使って ReadOnlySpan<byte> にマッチさせるのは無理。
// 「""u8 は const じゃないからパターンに使えない」みたいな理由。
// (C# 11 正式リリースでもこれはできないままな可能性大。)

static bool? error(ReadOnlySpan<byte> s) => s switch
{
    "true"u8 => true,
    "false"u8 => false,
    _ => null,
};
#endif
