using System;
using System.Runtime.CompilerServices;

Console.WriteLine("Main");

class C
{
    [ModuleInitializer]
    public static void M() => Console.WriteLine("ModuleInitializer");
}

namespace System.Runtime.CompilerServices
{
    class ModuleInitializerAttribute : Attribute { }
}
