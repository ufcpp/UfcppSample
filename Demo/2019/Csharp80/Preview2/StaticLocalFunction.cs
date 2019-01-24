namespace Preview2.StaticLocalFunction
{
    using System;

    class Program
    {
        static void Main()
        {
            NormalLocalFunction();
            StaticLocalFunction();
        }

        static void NormalLocalFunction()
        {
            int n = 0;

            // 通常、ローカル関数はこんな感じで外のローカル変数を拾える。
            // けどもうかつにそれをやってしまうと結構パフォーマンス的にはペナルティ食らうので、
            // 「ローカル変数を拾いたくない」という意図を表明できる文法が求められてた。
            int a(int x) => n * x;

            n = 2;
            Console.WriteLine(a(3)); //6
        }

        static void StaticLocalFunction()
        {
            // ローカル関数に static を付けると、ローカル変数をキャプチャできなくなる。
            static int a(int x) => 2 * x;

#if false
            // 以下のコードは2行目の n のところでエラーに。
            int n = 0;
            static int a(int x) => n * x;
#endif

            Console.WriteLine(a(3)); //6
        }
    }
}
