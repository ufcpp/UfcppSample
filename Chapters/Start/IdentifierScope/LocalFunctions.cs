namespace IdentifierScope.LocalFunctions
{
    class Program
    {
        static void Main()
        {
            // ローカル関数は宣言より前で使える
            var y = f(2);

            int f(int x) => x * x;
        }
    }

    namespace DefiniteAssignment
    {
        class Program
        {
            static void SuccessfulSample()
            {
                int a; // 未初期化
                int f(int x) => a * x; // (この時点で)未初期化変数 a 参照
                a = 10; // ここで初期化
                var y = f(2); // OK
            }

#if false
            static void ErroneousSample()
            {
                int a; // 未初期化
                int f(int x) => a * x; // 未初期化変数 a 参照
                // 初期化しない！
                var y = f(2); // コンパイル エラー
            }
#endif
        }
    }

    namespace LocalVariableOrMethod
    {
        using System;

        class Program
        {
            static void Main()
            {
                // ローカル関数は、こういうローカル変数的な扱いすべき？
                Func<int, int> f = x => x * x;

                // もしローカル変数的に扱うなら、f はこの後ろでしか使えない
                var y = f(2);

                // それとも、メソッドと同じような扱いにすべき？
                // メソッドなら、宣言よりも前でも使える
                var z = M(2);
            }

            // メソッドであれば、宣言が後ろにあってもいい
            static int M(int x) => x * x;
        }
    }
}
