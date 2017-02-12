using System;

namespace ConsoleApp1._03_LocalFunctions
{
    class Performance
    {
        public static void Run()
        {
            // ラムダ式でローカル変数をキャプチャすると、ヒープ確保発生
            // (コンパイラーがクラスを1個生成して、new する)
            var memory = GC.GetTotalMemory(false);

            for (int i = 0; i < 1000; i++)
            {
                int a = 0; // キャプチャするための適当なローカル変数
                Action<int> f = x => a += x;
                f(10);
            }

            Console.WriteLine(GC.GetTotalMemory(false) - memory);

            // ローカル関数でローカル変数をキャプチャすると発生しない
            // (コンパイラーが構造体を1個生成して、ref 渡しする)
            memory = GC.GetTotalMemory(false);

            for (int i = 0; i < 1000; i++)
            {
                int a = 0; // キャプチャするための適当なローカル変数
                void f(int x) => a += x;
                f(10);
            }

            Console.WriteLine(GC.GetTotalMemory(false) - memory);
        }
    }
}
