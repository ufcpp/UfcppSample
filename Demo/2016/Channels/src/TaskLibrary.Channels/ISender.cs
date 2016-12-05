using SystemAsync;
using System;
using System.Threading.Tasks;

namespace TaskLibrary.Channels
{
    /// <summary>
    /// イベントを起こす側のインターフェイス。
    /// </summary>
    /// <typeparam name="TMessage">イベント引数の型。</typeparam>
    public interface ISender<TMessage>
    {
        /// <summary>
        /// イベント購読。
        /// </summary>
        /// <param name="handler">イベントハンドラー。</param>
        /// <returns>購読解除用に使うdisposable。</returns>
        IDisposable Subscribe(AsyncAction<TMessage> handler);

        /// <summary>
        /// チャネルが閉じたときに完了するタスクを返す。
        /// </summary>
        Task Completed { get; }
    }
}
