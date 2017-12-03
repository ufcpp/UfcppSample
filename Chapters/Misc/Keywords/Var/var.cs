#pragma warning disable 219

namespace Keywords.Var
{
    class Inferred
    {
        static void F()
        {
            // この場合は型推論で Int 型の変数 var になる
            var var = 1;
        }
    }

    class SuccessfullyCompiled
    {
        struct var
        {
            public int value;
            public static implicit operator var(int n) => new var { value = n };
        }

        static void F()
        {
            // この場合は ↑ の var 構造体型の変数 var になる
            var var = 1;
        }
    }

    class Erroneous
    {
        struct var { }

        static void F()
        {
#if false
            // この場合は ↑ の var 構造体型になるけども、1 を代入できなくてコンパイル エラー
            var var = 1;
#endif
        }
    }
}
