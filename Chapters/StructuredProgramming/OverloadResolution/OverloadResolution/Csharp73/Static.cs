namespace OverloadResolution.Csharp73.Static
{
    using System;

    struct Static { }
    struct Instance { }

    class Program
    {
        // 既定値が入っているのでどちらも M() で呼べる。
        // 片方は静的メソッドで、もう片方はインスタンス メソッド。
        static void M(Static x = default) => Console.WriteLine("Static");
        void M(Instance x = default) => Console.WriteLine("Instance");

        static void Main()
        {
            // 型名.M() で呼べるのは静的メソッドだけのはず。
            // でも、これまでは、M(Static) か M(Instance) かの区別がつかなかった。
            // C# 7.3 では M(Static) が選ばれるように。
            Program.M();

            // インスタンス.M() で呼べるのはインスタンス メソッドだけのはず。
            // 同上。
            // C# 7.3 では M(Instance) が選ばれるように。
            new Program().M();

            // Main が静的メソッドなので、何もつけない場合、この M() も静的な方が呼ばれる。
            M();
        }

        void InstanceMethod()
        {
            // でも、これはダメ。
            // 静的な方もインスタンスの方も M() で呼べるので不明瞭。
#if Uncompilable
            M();
#endif

            // これなら OK。
            // this. が付いているのでインスタンス メソッドに絞られる。
            this.M();
        }
    }
}
