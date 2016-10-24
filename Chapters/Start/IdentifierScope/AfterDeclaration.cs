namespace IdentifierScope.AfterDeclaration
{
    using System;

    class Program
    {
#if false
        static void ReadBeforeDeclaration()
        {
            // 宣言より後なのでコンパイル エラー
            x = 10;

            int x; // 変数宣言

            // 宣言より後なので OK
            x = 20;
        }
#endif

        static void DefiniteAssignment1()
        {
#if false
            {
                int x; // 未初期化変数

                // 初期化前には読めない。コンパイル エラー
                Console.WriteLine(x);
            }
#endif

            {
                int y; // 未初期化変数

                y = 10; // ここで初期化

                // これならOK
                Console.WriteLine(y);
            }
        }

        static void DefiniteAssignment2()
        {
#if false
            {
                int x; // 未初期化変数

                if (Console.ReadKey().Key == ConsoleKey.A)
                {
                    x = 10;
                }

                // 条件を満たさない時に x が初期化されない。コンパイル エラー
                Console.WriteLine(x);
            }
#endif

            {
                int y; // 未初期化変数

                if (Console.ReadKey().Key == ConsoleKey.A)
                {
                    y = 10;
                }
                else
                {
                    y = 20;
                }

                // これならOK
                Console.WriteLine(y);
            }
        }
    }
}
