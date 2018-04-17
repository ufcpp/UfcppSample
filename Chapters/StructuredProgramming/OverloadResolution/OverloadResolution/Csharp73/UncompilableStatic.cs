namespace OverloadResolution.Csharp73.UncompilableStatic
{
    using System;

    struct Static { }
    struct Instance { }

    class Program
    {
        // 同名で、片方は静的メソッドで、もう片方はインスタンス メソッド。
        static void M(Static x) => Console.WriteLine("Static");
        void M(Instance x) => Console.WriteLine("Instance");

        static void Main()
        {
#if Uncompilable
            // 型名.M() で呼べるのは静的メソッドだけのはず。
            // でも、エラー メッセージとしては「M(Instance) を呼ぶにはインスタンスが必要」の類。
            Program.M(new Instance());

            // インスタンス.M() で呼べるのはインスタンス メソッドだけのはず。
            // でも、エラー メッセージとしては「M(Static) を呼ぶにはインスタンス越しじゃダメ」の類。
            new Program().M(new Static());

            // つまり、引数の型でのオーバーロード解決を先にやって、その後、静的/インスタンスの区別を調べてる。
#endif
        }
    }
}
