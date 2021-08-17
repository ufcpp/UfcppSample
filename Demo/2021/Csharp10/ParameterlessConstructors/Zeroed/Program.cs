Console.WriteLine(new Zeroed1(1).Y);
Console.WriteLine(new Zeroed2(1).Y);

// これは今までも書けた記法:
struct Zeroed1
{
    public int X;
    public int Y; // 警告は出る。

    public Zeroed1(int x) : this() // : this() を付けることで、「X も Y も 0 初期化」の意味があった。
    {
        X = x;
        // Y は 0 になってる。
    }
}

// C# 10.0 から:
struct Zeroed2
{
    public int X;
    public int Y;

    public Zeroed2(int x) : this() // この : this() は引数なしコンストラクター呼び出しの意味に。
    {
        X = x;
        // Y は 2 になってる。
    }

    public Zeroed2()
    {
        X = 1;
        Y = 2;
    }
}

// C# 10.0 から:
struct Zeroed3
{
    public int X;
    public int Y;

    public Zeroed3(int x)
    {
        this = default; // これまでの : this() と一番近い挙動をするのはこれ。
        X = x;
        // Y は 0 になってる。
    }

    public Zeroed3()
    {
        X = 1;
        Y = 2;
    }
}
