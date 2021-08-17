static T New<T>() where T : new() => new();

Console.WriteLine(New<Class>().X); // 0
Console.WriteLine(New<A>().X); // 0
Console.WriteLine(New<B>().X); // 1

// クラスの場合、何も書かないとき、暗黙的に引数なしコンストラクターが作られてる。
class Class
{
    public int X;
}

// 構造体の場合、何も書かないとき、「コンストラクターはない」という扱い。
// new T() は default(T) (0 初期化)と同じ意味。
struct A
{
    public int X;
}

// C# 10.0 で引数なしコンストラクターを書けるようになって…
// new T() と書くと引数なしコンストラクターが呼ばれる。
struct B
{
    public int X;
    public B() => X = 1;
}

class StructConstraint
{
    // struct 制約は暗黙的に new() 制約を含む。
    // 昔は new T() と default(T) が同じ意味だったけど、今は違う意味になる。
    static T New<T>() where T : struct => new(); // 引数なしコンストラクターがあればそれが呼ばれる。
    static T Default<T>() where T : struct => default; // 常に 0 初期化。

    public static void M()
    {
        Console.WriteLine(New<A>().X); // 0
        Console.WriteLine(New<B>().X); // 1
        Console.WriteLine(Default<A>().X); // 0
        Console.WriteLine(Default<B>().X); // 0
    }

    struct A
    {
        public int X;
    }

    struct B
    {
        public int X;
        public B() => X = 1;
    }
}
