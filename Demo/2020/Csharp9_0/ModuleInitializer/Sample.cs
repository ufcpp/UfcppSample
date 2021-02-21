using System;
using System.Runtime.CompilerServices;

class Sample
{
    [ModuleInitializer]
    public static void Init()
    {
        Console.WriteLine("必ず1回だけ呼ばれる");
    }

    public class C1
    {
        [ModuleInitializer]
        public static void Init1() { }

        [ModuleInitializer]
        public static void Init2() { }
    }

    public class C2
    {
        [ModuleInitializer]
        public static void Init1() { }
    }

#if ERROR
    public class Generic<T>
    {
        // これはコンパイル エラー。
        // 静的コンストラクターなら、 Generic<int> みたいな具象化した型ごとに呼ばれるけど、
        // モジュール初期化のタイミングでは何の型で具象化されるかわからなくて呼びようがない。
        [ModuleInitializer]
        public static void Init1() { }
    }
#endif
}
