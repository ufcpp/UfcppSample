namespace Patterns.NonNull
{
    class Program
    {
        struct LongLongNamedStruct { }

        void M1(LongLongNamedStruct? x)
        {
            // こういう書き方だと null チェックになる。
            if (x is LongLongNamedStruct nonNull)
            {
                // obj が null じゃない時だけここが実行される。
                // でも、x の型が既知なのに、長いクラス名をわざわざ書くのはしんどい…
            }
        }

        void M2(LongLongNamedStruct? x)
        {
            // が、var パターンは null にもマッチしちゃう。
            // (var は「何にでもマッチ」。null でも true になっちゃう。)
            if (x is var nullable)
            {
                // obj が null でもここが実行される。
            }
        }

        void M3(LongLongNamedStruct? x)
        {
            // (C# 8.0) プロパティ パターンであれば、null チェックを含む。
            if (x is { } nonNull)
            {
                // obj が null じゃない時だけここが実行される。
            }
        }
    }
}
