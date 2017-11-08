using System;
using System.Collections.Generic;
using System.Text;

namespace Span.SafeStackalloc
{
    class Program
    {
        static void Main()
        {
            Safe();
            Unsafe();
        }

        // Span 版 = safe
        static void Safe()
        {
            Span<byte> span = stackalloc byte[8];

            try
            {
                // 8バイトしか確保していないのに、9要素目に書き込み
                span[8] = 1;
            }
            catch(IndexOutOfRangeException)
            {
                // ちゃんと例外が発生してここに来る
                Console.WriteLine("span[8] はダメ");
            }
        }

        // ポインター版 = unsafe
        static unsafe void Unsafe()
        {
            byte* p = stackalloc byte[8];

            try
            {
                // 8バイトしか確保していないのに、9要素目に書き込み
                p[8] = 1;
            }
            catch (Exception)
            {
                // ここには来ない！
                // 結果、不正な場所に 1 が書き込まれてるはず(かなり危険)
                // それも、エラーを拾う手段がないので気づきにくい
                throw;
            }
        }

        static void ConditionalStackalloc(int bufferSize)
        {
            Span<byte> buffer = bufferSize <= 128 ? stackalloc byte[bufferSize] : new byte[bufferSize];
        }
    }
}
