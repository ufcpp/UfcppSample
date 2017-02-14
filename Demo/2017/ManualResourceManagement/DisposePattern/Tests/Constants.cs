using System;

namespace DisposePattern.Tests
{
    static class Constants
    {
        /// <summary>
        /// テスト用のコードを何並列で実行するか。
        /// </summary>
        public const int Parallelism = 10000;

        /// <summary>
        /// Delay 用に、ランダムな時間を作って返す。
        /// </summary>
        public static TimeSpan Time(this Random r) => TimeSpan.FromMilliseconds(r.NextDouble() * 30 + 1);
    }
}
