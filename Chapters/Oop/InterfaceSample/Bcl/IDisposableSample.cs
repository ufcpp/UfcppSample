namespace InterfaceSample.Bcl
{
    using System;

    /// <summary>
    /// <see cref="IDisposable"/> を実装している = 使い終わったら明示的に Dispose を呼ぶ必要がある。
    /// </summary>
    class Stopwatch : IDisposable
    {
        System.Diagnostics.Stopwatch _s = new System.Diagnostics.Stopwatch();

        public Stopwatch() { _s.Start(); }

        public void Dispose()
        {
            _s.Stop();
            Console.WriteLine(_s.Elapsed);
        }
    }

    class IDisposableSample
    {
        public static void Main()
        {
            // using ブロックを抜けたら自動的に Dispose が呼ばれる
            using (new Stopwatch())
            {
                var t = T(12, 6, 0);
            }
        }

        private static int T(int x, int y, int z) => x <= y ? y : T(T(x - 1, y, z), T(y - 1, z, x), T(z - 1, x, y));
    }
}
