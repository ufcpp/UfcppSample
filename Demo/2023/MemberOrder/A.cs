using System.Runtime.CompilerServices;

class A
{
    [ModuleInitializer]
    public static void Init() => Console.WriteLine("Latin A");
}
