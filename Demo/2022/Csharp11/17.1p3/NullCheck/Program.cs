#pragma warning disable IDE0060

//## 引数の後ろに !!

// 引数名の後ろに !! を付けることによって、引数の null チェックコードが挿入される。
void m(string s!!) { }

m("abc"); // OK

try
{
    // 本来、(#nullable enable な状況下で) m に null は渡しちゃいけないんだけど、
    // 警告を無視して立ち悪く null を渡すことが可能。
    m(null!);

    // この場合はこんなことするやつが悪いものの、古いコードを参照したり unsafe とか使ってると事故で紛れ込むこともある。
    // 要するに、#nullable enable (NRT) を100%信用してはいけない状況での次善策。

    // ちなみに、
    // ! は「人間が null 安全保証するように頑張るからとりあえず何もしないでくれ。null なまま dereference しないように人手で頑張る。」
    // !! は「NRT を無視して null を渡してくるやつがいるかもしれない。その場合は容赦なく例外を出してくれ。」
    // という感じで、似てるようで結果は真逆。
    // あと、「NRT を無視された場合の備え」なので、NRT とは相補的な機能。
    //
    // なので、同じ記号を使うのを避けて !! になった。
    // 書き方が string!! s とかじゃないのも「型がらみの機能じゃないから」(型名にくっついてると「string!! 型」と勘違いされる)。
}
catch (ArgumentNullException)
{
    Console.WriteLine("ここに来るはず。");
}

//## !! の展開結果の説明

// m と近しい手書きコード:
m1("");

// 完全に手書き。
void m1(string s)
{
    // ただ、throw はインライン展開を阻害しがちなので、
    // 実はこんな感じで m1 内に直接 throw を書くのはちょっとだけパフォーマンスに不利だったりする。
    if (s is null) throw new ArgumentNullException(nameof(s));
}

m2("");

// .NET 6 から入った ArgumentNullException.ThrowIfNull を活用。
void m2(string s)
{
    // C# 10 の新機能である CallerArgumentExpression を使ってて、引数名の "s" は自動的に ThrowIfNull に渡る。
    // C# 11 の s!! で早速あんまり使い道がなくなったけども… 
    ArgumentNullException.ThrowIfNull(s);
}

//## null チェックのタイミング

// ちなみに、!! で挿入される null チェックは、手書きではどうやっても書けないくらい「早いタイミング」で呼ばれる。
try
{
    // base() もフィールド初期化子も呼ばれない。
    // それよりも先に throw new ArgumentNullException。
    new Derived(null!);
}
catch { }

class Base
{
    public Base()
    {
        Console.WriteLine("Base コンストラクターが呼ばれた");
    }
}

class Derived : Base
{
    // !! で挿入した null チェックは base() よりも前に呼ばれる。
    public Derived(string s!!) : base()
    {
        // ここに if (s is null) throw を書くのだと base() 呼び出しよりも後になる。
    }

    public int X = InitX();

    private static int InitX()
    {
        Console.WriteLine("Derived フィールド初期化子が呼ばれた");
        return 1;
    }
}
