namespace DefaultValue
{
    // C# 6.0 で、一瞬、構造体にも引数なしのコンストラクターを持てるようにしようという話が出た。
    // 結局は没に。
    // 今でも「やりたい」という意思は残っているものの、どうしようもない不具合を抱えてて進めれないみたい。
    //
    // 問題は
    // - 長らく構造体の new T() は default(T) と同じ意味という前提でやってきすぎた
    // - リフレクションを使うライブラリで、結構な数のものがこの前提で最適化を掛けちゃってて、そういうやつの挙動が壊れる
    // - というか、.NET Framework 自体、Activator.CreateInstance<T>() がコンストラクターを呼んでくれない挙動になってた
    // という感じ。

#if 没
    struct Point
    {
        public int X;
        public int Y;

        // C# 5.0 以前ではコンパイル エラーになる
        public Point()
        {
            X = int.MinValue;
            Y = int.MinValue;
        }
    }

    struct Entry
    {
        public int Id = 0;       // C# 5.0 以前ではコンパイル エラーになる
        public string Name = ""; // ここも
    }
#endif
}
