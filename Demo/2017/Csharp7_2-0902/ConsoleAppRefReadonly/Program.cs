using System;
using System.Collections.Generic;
using System.Threading.Tasks;

struct Vector2D
{
    public float X;
    public float Y;

    public Vector2D(float x, float y)
    {
        X = x;
        Y = y;
    }
}

static class Extensions
{
    public static float Dot(in this Vector2D p, in Vector2D q) => p.X * q.X + p.Y * q.Y;
    public static float Cross(in this Vector2D p, in Vector2D q) => p.X * q.Y - p.Y * q.X;
}

class Program
{
    static void Main(string[] args)
    {
        RefExtensionMethods();
        SafeStackalloc();
    }

    private static void RefExtensionMethods()
    {
        var p = new Vector2D(1, 2);
        var q = new Vector2D(3, 4);

        Console.WriteLine(p.Dot(q));
        Console.WriteLine(p.Cross(q));
    }

    // 特に unsafe を付けてない
    private static void SafeStackalloc()
    {
        // safe な文脈で stackalloc！
        Span<byte> buf = stackalloc byte[8];
        for (int i = 0; i < buf.Length; i++)
        {
            buf[i] = (byte)((i + 1) * 0x11);
        }

        var x = buf.NonPortableCast<byte, ulong>()[0];
        Console.WriteLine(x.ToString("X")); // 8877665544332211
    }

    static IEnumerable<int> Iterator()
    {
        Span<byte> local = new byte[1]; // OK

        yield return 0;
#if NG
            Span<byte> captured = new byte[1]; // NG
            yield return 0;
            captured = captured.Slice(1);
#endif
    }

    static async Task<int> Async()
    {
#if NG
            Span<byte> local = new byte[1]; // NG
#endif

        await Task.Delay(1);

        return 1;
    }
}
