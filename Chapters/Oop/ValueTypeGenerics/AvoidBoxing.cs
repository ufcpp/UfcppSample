namespace ValueTypeGenerics.AvoidBoxing
{
    using System;

    // 無駄なヒープ確保をしないように構造体に
    struct Disposable : IDisposable
    {
        public void Dispose() { }
    }

    class Program
    {
        static void WithInterface(IDisposable x) => x.Dispose();

        // やってることは WithInterface を同じに見えて…
        static void WithGenerics<T>(T x)
            where T : IDisposable
            => x.Dispose();

        static void Main()
        {
            // 構造体なので無駄なヒープ確保はしない
            default(Disposable).Dispose();

            for (int i = 0; i < 10000; i++)
            {
                // ところが、インターフェイスを介するとボックス化を起こす
                // 無駄なヒープ確保に
                // 1個や2個なら大したコストではないものの、何度も呼ばれるとさすがにつらい
                WithInterface(default(Disposable));
            }

            for (int i = 0; i < 10000; i++)
            {
                // ジェネリックを介するとボックス化が不要
                // 繰り返し呼んでも平気
                WithGenerics(default(Disposable));
            }
        }
    }
}
