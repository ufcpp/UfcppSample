using SystemAsync;
using System;
using System.Threading.Tasks;

namespace TaskLibrary.Channels
{
    /// <summary>
    /// イベント処理の前後をフックして、別の処理を行うチャネル。
    /// </summary>
    /// <typeparam name="T">メッセージの型。</typeparam>
    /// <remarks>
    /// 複数個のハンドラーを<see cref="ISender{TMessage}.Subscribe(AsyncAction{TMessage})"/>するのだと、実行順が不定なので。
    /// 必ず最初に、必ず最後にやりたいイベント処理にはこのクラスを使う。
    /// </remarks>
    public class LoggingChannel<T> : ISender<T>, IDisposable
    {
        private ISender<T> _inner;
        private IDisposable _subscription;

        /// <summary>
        /// </summary>
        /// <param name="sender">メッセージ送信元。</param>
        /// <param name="beforeMessage">イベント発生前に挟みたい処理。</param>
        /// <param name="afterMessage">イベント発生後に挟みたい処理。</param>
        public LoggingChannel(ISender<T> sender, AsyncAction<T> beforeMessage, AsyncAction<T> afterMessage, InvocationMode mode = InvocationMode.Parallel)
        {
            _inner = sender;

            _subscription = _inner.Subscribe(async (x, ct) =>
            {
                if (beforeMessage != null) await beforeMessage(x, ct);
                await _handlers.InvokeAsync(mode, x, ct);
                if (afterMessage != null) await afterMessage(x, ct);
            });
        }

        /// <summary><see cref="ISender{TMessage}"/></summary>
        public Task Completed => _inner.Completed;

        private AsyncActionList<T> _handlers;

        /// <summary><see cref="ISender{TMessage}"/></summary>
        public IDisposable Subscribe(AsyncAction<T> handler)
        {
            _handlers.Add(handler);
            return Disposable.Create(() => _handlers.Remove(handler));
        }

        public void Dispose() => _subscription.Dispose();
    }
}
