using System;
using System.Threading.Tasks;

namespace SystemAsync
{
    /// <summary>
    /// <see cref="IAsyncDisposable"/> 関連、ユーティリティ メソッドや拡張。
    /// </summary>
    public static class AsyncDisposable
    {
        /// <summary>
        /// <see cref="Func{Task}"/> から <see cref="IAsyncDisposable"/> を作る。
        /// </summary>
        /// <param name="disposeAsync"><see cref="IAsyncDisposable.DisposeAsync"/> で呼びたい処理。</param>
        /// <returns><see cref="IAsyncDisposable"/> 化したもの。</returns>
        public static IAsyncDisposable Create(Func<Task> disposeAsync) => new AsyncActionDisposer(disposeAsync);
    }

    internal class AsyncActionDisposer : IAsyncDisposable
    {
        Func<Task> _onDispose;

        public AsyncActionDisposer(Func<Task> onDispose)
        {
            if (onDispose == null)
                throw new ArgumentNullException();
            _onDispose = onDispose;
        }

        public Task DisposeAsync() => _onDispose();
    }
}
