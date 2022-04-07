#pragma warning disable CS8321, IDE0051, IDE0062, IDE0090

// 文法的には何も変化がないものの、内部的な最適化の話。

// こんなメソッドがあったとして、
static int square(int x) => x * x;

// こんな感じでデリゲート化したいとする。
Func<int, int> f() => square;

// f とほぼ等価なコード、昔はこうだった:
// 毎回 Func デリゲートが new されて、毎回アロケーションが掛かる。
Func<int, int> f0() => new Func<int, int>(square);

// ちなみに、ラムダ式の場合はキャッシュされる仕様なので、毎回はアロケーション掛からない。
Func<int, int> fLambda() => x => x * x;

partial class Program
{
    static int square(int x) => x * x;

    // C# 11 では、これに類するコード生成されて、上記の f でもキャッシュが掛かるようになった。
    static Func<int, int>? _fCache;
    static Func<int, int> f() => _fCache ??= new Func<int, int>(square);
}
