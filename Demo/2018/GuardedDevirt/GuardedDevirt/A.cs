using System;
using System.Linq;
using System.Runtime.CompilerServices;

// A 群。
// M の実装が、インライン展開できない程度のサイズという想定のもの。
// (今回は NoInlining オプションで代用。)
//
// かつ、A1～A4 の発生確率が均等で交互(= 分岐予測が効きにくい)。
static class A
{
    public static I[] GetData() => Enumerable.Range(0, 1000).SelectMany(_ => new I[]
    {
        new A1(), new A2(), new A3(), new A4(),
        new A1(), new A2(), new A3(), new A4(),
        new A1(), new A2(), new A3(), new A4(),
        new A1(), new A2(), new A3(), new A4(),
    }).ToArray();
}

struct A1 : I
{
    [MethodImpl(MethodImplOptions.NoInlining)]
    public void M()
#if DEBUG
        => Console.WriteLine("A1");
#else
    { }
#endif
}

struct A2 : I
{
    [MethodImpl(MethodImplOptions.NoInlining)]
    public void M()
#if DEBUG
        => Console.WriteLine("A2");
#else
    { }
#endif
}

struct A3 : I
{
    [MethodImpl(MethodImplOptions.NoInlining)]
    public void M()
#if DEBUG
        => Console.WriteLine("A3");
#else
    { }
#endif
}

struct A4 : I
{
    [MethodImpl(MethodImplOptions.NoInlining)]
    public void M()
#if DEBUG
        => Console.WriteLine("A4");
#else
    { }
#endif
}
