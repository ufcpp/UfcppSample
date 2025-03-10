using ConsoleApp1;
using System.Runtime.CompilerServices;

M(1, 2);
M("1", 2.0);
M(true, DateTimeOffset.Now);
M((byte)1, (short)2, (ushort?)3);

partial class Program
{
    public static void M<T1, T2>(T1 x1, T2 x2)
    {
        var tuple = (x1, x2);
        var accessor = Accessor.Create(ref tuple);
        A(tuple); // boxed
        B(accessor);
        Console.WriteLine();
    }

    public static void M<T1, T2, T3>(T1 x1, T2 x2, T3 x3)
    {
        var tuple = (x1, x2, x3);
        var accessor = Accessor.Create(ref tuple);
        A(tuple); // boxed
        B(accessor);
    }

    static void A(ITuple tuple)
    {
        for (int i = 0; i < tuple.Length; i++)
        {
            Console.WriteLine(tuple[i]?.GetType().Name);
        }
    }

    static void B<TAccessor>(TAccessor tuple)
        where TAccessor : struct, ITupleAccessor, allows ref struct
    {
        foreach (var t in tuple)
        {
            Console.WriteLine(t.Type.Name);
        }
    }
}
