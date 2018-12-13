namespace Span.Pointer
{
    using System;

    // unsafe を使うと速い処理の典型例として、一定範囲を 0 クリアする処理
    class Program
    {
        // 作る側
        // ライブラリを作る側としては別に unsafe コードがあっても不都合はそこまでない
        static unsafe void Clear(byte* p, int length)
        {
            var last = p + length;
            while (p + 8 < last)
            {
                *(ulong*)p = 0;
                p += 8;
            }
            if (p + 4 < last)
            {
                *(uint*)p = 0;
                p += 4;
            }
            while (p < last)
            {
                *p = 0;
                ++p;
            }
        }

        // 使う側
        static void Main()
        {
            var array = new byte[256];

            // array をいろいろ書き換えた後、全要素 0 にクリアしたいとして
            for (int i = 0; i < array.Length; i++) array[i] = (byte)(i + 1);

            // ライブラリを使う側に unsafe が必要なのは怖いし面倒
            unsafe
            {
                fixed (byte* p = array)
                    Clear(p, array.Length);
            }

            Console.WriteLine(string.Join("", array));
        }

        // 使う側に unsafe を求めないために要するオーバーロードいろいろ
        static void Clear(ArraySegment<byte> segment) => Clear(segment.Array, segment.Offset, segment.Count);
        static void Clear(byte[] array, int offset = 0) => Clear(array, offset, array.Length - offset);
        static void Clear(byte[] array, int offset, int length)
        {
            unsafe
            {
                fixed (byte* p = array)
                {
                    Clear(p + offset, length);
                }
            }
        }
    }
}

namespace Span.Pointer.Copy
{
    using System;

    class Program
    {
        // Clear は1つしか引数がないのでまだマシ。
        // コピー(コピー元とコピー先)とか、2つになるとだいぶ面倒に。

        static void Copy(ArraySegment<byte> source, ArraySegment<byte> destination)
            => Copy(source.Array, source.Offset, destination.Array, destination.Offset, source.Count);
        static void Copy(byte[] source, int sourceOffset, byte[] destination, int destinationOffset)
            => Copy(source, sourceOffset, destination, destinationOffset, source.Length - sourceOffset);
        static void Copy(byte[] source, int sourceOffset, byte[] destination, int destinationOffset, int length)
        {
            unsafe
            {
                fixed (byte* s = source)
                fixed (byte* d = destination)
                {
                    Copy(s + sourceOffset, d + destinationOffset, length);
                }
            }
        }
        // 他にも、利便性を求めるなら、
        // source, destination の片方だけが ArraySegment のパターンとか
        // 片方だけがポインターのパターンとか(組み合わせなのでパターンが多くなる)

        static unsafe void Copy(byte* source, byte* destination, int length)
        {
            var last = source + length;
            while (source + 8 < last)
            {
                *(ulong*)source = *(ulong*)destination;
                source += 8;
            }
            if (source + 4 < last)
            {
                *(uint*)source = *(uint*)destination;
                source += 4;
            }
            while (source < last)
            {
                *source = *destination;
                ++source;
            }
        }
    }
}

namespace Span.Pointer.Span
{
    using System;

    class Program
    {
        // 作る側
        // Span<T> なら配列でもポインターでも、その全体でも一部分でも受け取れる
        static void Clear(Span<byte> span)
        {
            unsafe
            {
                // 結局内部的には unsafe にしてポインターを使った方が速い場合あり
                fixed (byte* pin = span)
                {
                    var p = pin;
                    var last = p + span.Length;
                    while (p + 8 < last)
                    {
                        *(ulong*)p = 0;
                        p += 8;
                    }
                    if (p + 4 < last)
                    {
                        *(uint*)p = 0;
                        p += 4;
                    }
                    while (p < last)
                    {
                        *p = 0;
                        ++p;
                    }
                }
            }
        }

        // 使う側
        static void Main()
        {
            var array = new byte[256];

            // array をいろいろ書き換えた後、全要素 0 にクリアしたいとして
            for (int i = 0; i < array.Length; i++) array[i] = (byte)(i + 1);

            // 呼ぶのがだいぶ楽
            Clear(array);

            Console.WriteLine(string.Join("", array));
        }
    }
}
