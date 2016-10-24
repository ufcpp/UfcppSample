namespace IdentifierScope.EmbeddedStatement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    class Program
    {
        static void DeclarationInEmbeddedStatement()
        {
#if false
            if (true)
                int x = 10; // コンパイル エラー
#endif

            if (true)
            {
                int x = 10; // これなら OK
            }

#if false
            foreach (var n in new[] { 1 })
                int x = 10; // コンパイル エラー
#endif

            foreach (var n in new[] { 1 })
            {
                int x = 10; // これなら OK
            }
        }

        static void M(object obj)
        {
            if (obj is int x1) // 条件式内
                ;

            foreach (var n in obj is int x2 ? "a" : "b") // foreach の () 内
                ;

            for (var n = 0; obj is int x3 ? n < x3 : false; n++) // for の () 内
                ;

            if (true)
                Console.WriteLine(obj is int x4 ? 1 : 2); // 埋め込みステートメント内

            foreach (var n in "a")
                Console.WriteLine(obj is int x5 ? 1 : 2); // 埋め込みステートメント内
        }

        static int SwitchCaseSample(object obj)
        {
            switch (obj)
            {
                case int x: return x;
                case string x: return x.Length; // int x の方とは別になる
                default: throw new IndexOutOfRangeException();
            }
        }

        static void ErroneousSample(object obj)
        {
            if (true)
            {
                Console.WriteLine(obj is int x ? 1 : 2); // もちろん、ブロック内がスコープ
                x = 1; // これは OK
            }

            if (true)
                Console.WriteLine(obj is int x ? 1 : 2); // 埋め込みステートメント内がスコープ

            foreach (var n in obj is int x ? "a" : "b") // foreach 内がスコープ
                ;

            for (var n = 0; obj is int x ? n < x : false; n++) // for 内がスコープ
                ;

            using (obj is IDisposable x ? x : null) // using 内がスコープ
                ;

#if false
            // どの x ももうスコープ外。コンパイル エラー
            x = 10;
#endif
        }

        static void SuccessfulSample(object obj)
        {
            if (obj is int x1) // 条件式内
            {
            }
            else
            {
                x1 = 10; // ここも x1 のスコープ
            }

            Console.WriteLine(x1); // ここも x1 のスコープ

            while (obj is int x2)
            {
                obj = "";
            }

            x2 = 1;// ここも x2 のスコープ
        }

        static int _field = int.TryParse("123", out var x) ? x : 0;
    }
}
