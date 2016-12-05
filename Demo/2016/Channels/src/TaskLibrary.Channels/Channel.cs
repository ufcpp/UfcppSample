using System;
using System.Threading;
using System.Threading.Tasks;
using SystemAsync;

namespace TaskLibrary.Channels
{
    /// <summary>
    /// イベントの送信元と受信先の間に挟まるクラス。
    /// Rxでいう<see cref="System.Reactive.Subjects.Subject{T}"/>。
    /// </summary>
    /// <typeparam name="TMessage">メッセージ(イベント引数)の型。</typeparam>
    public class Channel<TMessage> : IReceiver<TMessage>, ISender<Holder<TMessage>>
    {
        /// <summary>
        /// 複数のハンドラーが呼ばれた場合に、どう呼び出すか。
        /// </summary>
        public InvocationMode InvocationMode { get; }

        /// <summary>
        /// </summary>
        /// <param name="mode"><see cref="InvocationMode"/></param>
        public Channel(InvocationMode mode = InvocationMode.Parallel)
        {
            InvocationMode = mode;
        }

        /// <summary><see cref="IReceiver{TMessage}"/></summary>
        public Task OnNext(Holder<TMessage> message, CancellationToken ct)
            =>  _handlers.InvokeAsync(InvocationMode, message, ct);

        private AsyncActionList<Holder<TMessage>> _handlers;

        /// <summary><see cref="ISender{TMessage}"/></summary>
        public IDisposable Subscribe(AsyncAction<Holder<TMessage>> handler)
        {
            _handlers.Add(handler);
            return Disposable.Create(() => _handlers.Remove(handler));
        }

        /// <summary><see cref="ISender{TMessage}"/></summary>
        public Task Completed { get; private set; }

        /// <summary><see cref="IReceiver{TMessage}"/></summary>
        public void SetCompletion(Task completion) => Completed = completion;
    }
}
