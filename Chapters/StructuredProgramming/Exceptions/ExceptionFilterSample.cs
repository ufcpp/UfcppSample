using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Exceptions
{
    class ExceptionFilterSample
    {
        public static void Main()
        {
            ParallelCatch();

            try
            {
                F();
            }
            catch (DirectoryNotFoundException e)
            {
                // DirectoryNotFoundException のときと FileNotFoundException の時で
                // 全く同じ例外処理の仕方をしたい場合がある。
                Console.WriteLine(e);
            }
            catch (FileNotFoundException e)
            {
                // DirectoryNotFoundException のときと FileNotFoundException の時で
                // 全く同じ例外処理の仕方をしたい場合がある。
                Console.WriteLine(e);

                // コピペ コードになっちゃうので嫌！
            }

            try
            {
                F();
            }
            catch (Exception e) when (e is DirectoryNotFoundException || e is FileNotFoundException)
            {
                // DirectoryNotFoundException のときと FileNotFoundException の時で
                // 全く同じ例外処理の仕方をしたい場合がある。
                Console.WriteLine(e);
            }
        }

        private static void ParallelCatch()
        {
            try
            {
                Parallel.For(0, 10000, F);
            }
            catch (AggregateException e) when (e.InnerExceptions.Any(i => i is ArgumentException))
            {
                // F が ArgumentException を throw する場合でも、
                // Parellel.For を通した結果、ここに来る例外は AggregateException で、
                // AggregateException.InnerExceptions の中に ArgumentException が入っている。
            }
        }

        static void F(int n)
        {
            if (n < 0) throw new ArgumentException($"'{nameof(n)}' must be positive or zero");
        }

        static void F()
        {
        }
    }
}
