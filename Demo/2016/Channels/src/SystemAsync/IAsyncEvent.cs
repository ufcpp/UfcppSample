using System;

namespace SystemAsync
{
    /// <summary>
    /// イベント処理を非同期にやって、
    /// それをイベント送信側で await したい場合に使う。
    /// Rx 的にイベント処理するのの非同期ハンドリング版。
    /// </summary>
    /// <typeparam name="TEventArgs"></typeparam>
    public interface IAsyncEvent<TEventArgs> : IDisposable
    {
        /// <summary>
        /// イベント購読。
        /// Rx の Subscribe 的なもの。
        /// </summary>
        /// <param name="handler">イベント ハンドラー。</param>
        /// <returns>購読解除用の <see cref="IDisposable"/>。</returns>
        IDisposable Subscribe(AsyncHandler<TEventArgs> handler);
    }
}
