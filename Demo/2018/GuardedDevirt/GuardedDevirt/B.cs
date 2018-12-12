using System;
using System.Linq;
using System.Runtime.CompilerServices;

// B 群。
// M の実装が、インライン展開できるサイズという想定のもの。
//
// かつ、ほとんどが B1 で、残りの発生確率低め。
static class B
{
    public static I[] GetData() => Enumerable.Range(0, 1000).SelectMany(_ => new I[]
    {
        new B1(), new B1(), new B3(), new B1(),
        new B1(), new B2(), new B1(), new B1(),
        new B1(), new B1(), new B1(), new B1(),
        new B1(), new B1(), new B1(), new B4(),
    }).ToArray();
}

struct B1 : I
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void M()
#if DEBUG
        => Console.WriteLine("B1");
#else
    { }
#endif
}

struct B2 : I
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void M()
#if DEBUG
        => Console.WriteLine("B2");
#else
    { }
#endif
}

struct B3 : I
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void M()
#if DEBUG
        => Console.WriteLine("B3");
#else
    { }
#endif
}

struct B4 : I
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void M()
#if DEBUG
        => Console.WriteLine("B4");
#else
    { }
#endif
}
