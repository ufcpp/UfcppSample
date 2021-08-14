namespace ParameterlessConstructors;

class GenericNew
{
    public static T New<T>() where T : new() => new T();

    public static void M()
    {
        Console.WriteLine(New<Class>().X); // 0
        Console.WriteLine(New<NoConstructor>().X); // 0
        Console.WriteLine(New<HasConstructor>().X); // 1
    }

    // クラスの場合、何も書かないとき、暗黙的に引数なしコンストラクターが作られてる。
    class Class
    {
        public int X;
    }

    // 構造体の場合、何も書かないとき、「コンストラクターはない」という扱い。
    // new T() は default(T) (0 初期化)と同じ意味。
    struct NoConstructor
    {
        public int X;
    }

    // C# 10.0 で引数なしコンストラクターを書けるようになって…
    // new T() と書くと引数なしコンストラクターが呼ばれる。
    struct HasConstructor
    {
        public int X;
        public HasConstructor() => X = 1;
    }
}
