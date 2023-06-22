using System.Runtime.CompilerServices;

class B
{
    [ModuleInitializer]
    public static void Init() => Console.WriteLine("Latin B");
}
