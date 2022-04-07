using System.Text.Json;

//## raw string literals

// - 3個以上の " から開始
// - 複数行の場合、最初と最後の改行は無視される
// - " とか "" をエスケープなしで書ける(開始文字を """" とかにすれば、""" もエスケープなしで書ける)
// - 最後の """ 行に合わせてインデントが揃えられる(それより前の空白は無視される)
JsonDocument.Parse("""
    {
        "id": 123,
        "name": "abc",
        "data": [ true, null, 1.23 ]
    }
    """);

// これと等価なものを @"" で書くなら:
JsonDocument.Parse(@"{
    ""id"": 123,
    ""name"": ""abc"",
    ""data"": [ true, null, 1.23 ]
}");

//## 文字列長の例

// 1
Console.WriteLine("""
    a
    """.Length);

// 0
Console.WriteLine("""
    
    """.Length);

// ちなみに、↓はコンパイルエラーになる。そりゃ。↑ の「長さ0文字列」よりも短くなっちゃう。
#if false
Console.WriteLine("""
    """.Length);
#endif

// """ の行のインデント下げると…
// 5
Console.WriteLine("""
    a
""".Length);

//## 単一行 raw string

// 改行なしなら普通に単一行リテラルに:
// 単に " のエスケープが要らないだけ。
JsonDocument.Parse("""[true, null, "abc", 1.23]""");

//## " たくさん

// 開始を " 7個にしてみる。
Console.WriteLine("""""""
    """""" エスケープなしで " が6個
    """"" エスケープなしで " が5個
    """" エスケープなしで " が4個
    """ エスケープなしで " が3個
    "" エスケープなしで " が2個
    " エスケープなしで " が1個
    """"""");

//## interpolated raw string

// - $ を付けて、interpolation もできる
// - $ も任意個数付けれる
//   - $ と同じ数だけ { } が必要
//   - 例えば $$""" 開始だと、 {{ と }} で囲った場所が interpolation hole
//   - この場合、1個だけの { と } はエスケープなしで書ける

Console.WriteLine(f(1000, "abc")); // { "id": 1000, "name": "abc" } (空白文字除く)

static string f(int id, string name) => $$"""
    {
        "id": {{id}},
        "name": "{{name}}"
    }
    """;
