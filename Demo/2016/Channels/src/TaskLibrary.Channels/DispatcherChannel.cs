using SystemAsync;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TaskLibrary.Channels
{
    /// <summary>
    /// 同期コンテキストを拾って、<see cref="SynchronizationContext.Post(SendOrPostCallback, object)"/>内でイベントハンドラーを実行するチャネル。
    /// </summary>
    /// <typeparam name="T">メッセージの型。</typeparam>
    public class DispatcherChannel<T> : ISender<T>
    {
        private ISender<T> _inner;
        private SynchronizationContext _context;

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="context">ハンドラーを実行したい同期コンテキスト。null を渡すと、コンストラクター内で<see cref="SynchronizationContext.Current"/>を拾う。</param>
        public DispatcherChannel(ISender<T> sender, SynchronizationContext context = null)
        {
            _context = context ?? SynchronizationContext.Current;
            _inner = sender;
        }

        /// <summary><see cref="ISender{TMessage}"/></summary>
        public IDisposable Subscribe(AsyncAction<T> handler)
            => _inner.Subscribe(async (message, ct) =>
            {
                await _context.PostIfNotCurrentAsync(() => handler(message, ct)).ConfigureAwait(false);

                // ↑のPostの実装的に、スレッドプール上で実行してても以降の処理がUIスレッドに移っちゃったりする。
                // この空 await はちょっといまいちだけども…
                await Task.Run(() => { }).ConfigureAwait(false);
            });

        /// <summary><see cref="ISender{TMessage}"/></summary>
        public Task Completed => _inner.Completed;
    }

    public static partial class Channel
    {
        /// <summary>
        /// 拡張メソッドで<see cref="DispatcherChannel{T}"/>生成。
        /// </summary>
        public static DispatcherChannel<T> ObserveOn<T>(this ISender<T> sender, SynchronizationContext context = null)
            => new DispatcherChannel<T>(sender, context);
    }
}
