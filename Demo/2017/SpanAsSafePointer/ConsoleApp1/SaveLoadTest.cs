using System;
using Xunit;

public class SaveLoadTest
{
    private const int N = 3;
    private Point[] _source;

    public SaveLoadTest()
    {
        _source = new Point[N];

        var r = new Random();
        for (int i = 0; i < _source.Length; i++)
        {
            _source[i] = new Point(r.NextDouble(), r.NextDouble(), r.NextDouble());
        }
    }

    private static void AssertAll(Point[] a, Point[] b)
    {
        for (int i = 0; i < a.Length; i++)
        {
            Assert.Equal(a[i].X, b[i].X);
            Assert.Equal(a[i].Y, b[i].Y);
            Assert.Equal(a[i].Z, b[i].Z);
        }
    }

    private void CopyTest<T>()
        where T : struct, IPointCopier
    {
        var dest = new Point[_source.Length];
        default(T).Copy(_source, dest);
        AssertAll(_source, dest);
    }

    [Fact]
    public void Safe() => CopyTest<ConsoleApp1.Safe.Copier>();
    [Fact]
    public void Unsafe1() => CopyTest<ConsoleApp1.Unsafe1.Copier>();
    [Fact]
    public void Unsafe2() => CopyTest<ConsoleApp1.Unsafe2.Copier>();
    [Fact]
    public void SpanBase() => CopyTest<ConsoleApp1.SpanBase.Copier>();
}
