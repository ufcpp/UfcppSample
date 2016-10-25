namespace TaskLike
{
    using System;
    using System.Threading.Tasks;

    class Program
    {
        static async ValueTask<int> XAsync(Random r)
        {
            if (r.NextDouble() < 0.99)
            {
                // 99% ここを通る。
                // この場合、await が1度もなく、非同期処理にならない。
                // 非同期処理じゃないのに Task<int> のインスタンスが作られるのはもったいない
                return 1;
            }

            // こちら側は本当に非同期処理なので、Task<int> が必要。
            await Task.Delay(100);
            return 0;
        }

        static Task<int> _cache;

        // キャッシュしてるものなので、少し時間がたてば、確実に完了済みになる。
        static Task<int> CachedX => _cache ?? (_cache = Task.Run(() => 1));

        // 完了済みだと非同期処理にならない。
        // 非同期処理じゃないのに Task<int> のインスタンスが作られるのはもったいない
        static async ValueTask<int> Y() => await CachedX;
        static async ValueTask<int> Z() => await Y();
    }
}
