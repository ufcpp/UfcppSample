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
}
