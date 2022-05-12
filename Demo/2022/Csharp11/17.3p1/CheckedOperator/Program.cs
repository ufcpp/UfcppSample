// 通常(オプション指定がないとき、unchecked 扱い)、整数型の算術演算はオーバーフローチェックしない。
var x = new Int4Bit(3);
++x;
Console.WriteLine(x.Value);

// 同じ処理でも checked ブロックで囲うと…
try
{
    var y = new Int4Bit(3);

    checked
    {
        ++y;
    }
}
catch
{
    // ここに来るはず。
    Console.WriteLine("overflow");
}

//## checked operator の定義
// こんな感じの特殊用途な整数型を作ったとして
struct Int4Bit
{
    private readonly byte _value;
    public Int4Bit(int value) => _value = (byte)(value & 3);
    public int Value => _value;

    // デモ用に1個だけ演算子を実装。とりあえず簡単な ++ を。
    public static Int4Bit operator ++(Int4Bit x) => new(x._value + 1);

    // この型、このままだと int とかの組み込み型と違って、「オーバーフローした時の挙動」を選べない。
    // 組み込み型に準ずるユーザー定義型を作りたいのにできないことがあった。

    // generic math も導入されることだし、この「できないことがある」問題解消の動きが出てきた。
    // operator checked って文法で、 unchecked/checkd で別実装を持てる。
    public static Int4Bit operator checked ++(Int4Bit x) => x._value < 3 ? new(x._value + 1) : throw new Exception();
}

// dotnet/runtime で System.Int128 を足すみたいな話が出てるみたい。
// (この文脈で checked operator の話が出て採用された。)
// https://github.com/dotnet/runtime/pull/69204
// https://github.com/dotnet/runtime/issues/67151
