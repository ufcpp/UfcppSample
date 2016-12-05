using System;
using System.Linq;
using System.Threading.Tasks;
using SystemAsync;

namespace TaskLibrary.Channels
{
    /// <summary>
    /// 宛先ID ＋ メッセージ。
    /// </summary>
    /// <typeparam name="TMessage">メッセージ(イベント引数)の型。</typeparam>
    public struct DistributiveMessage<TMessage>
    {
        /// <summary>
        /// 宛先ID。
        /// null の時は全体宛。
        /// </summary>
        public int? Address { get; }

        /// <summary>
        /// 元々のメッセージ。
        /// </summary>
        public TMessage Message { get; }

        /// <summary></summary>
        /// <param name="address"><see cref="Address"/></param>
        /// <param name="message"><see cref="Message"/></param>
        public DistributiveMessage(int? address, TMessage message)
        {
            Address = address;
            Message = message;
        }
    }

    /// <summary>
    /// 並列メッセージ送信(<see cref="IReceiver{TMessage}.OnNext(Holder{TMessage}, System.Threading.CancellationToken)"/>に配列がわたってきてる)の時に、
    /// 送付先を分配する(配列をばらして、1個1個ばらばらに<see cref="ISender{TMessage}.Subscribe(Async.AsyncAction{TMessage})"/>したハンドラーに送る)。
    /// </summary>
    /// <typeparam name="TMessage">メッセージ(イベント引数)の型。</typeparam>
    /// <remarks>
    /// コンストラクターで引数に渡した<see cref="ISender{TMessage}"/>をsubscribeする。
    /// 購読解除するためには<see cref="IDisposable.Dispose"/>を呼ぶ。
    /// </remarks>
    public class DistributiveChannel<TMessage> : ISender<DistributiveMessage<TMessage>>
    {
        private ISender<Holder<TMessage>> _inner;

        /// <summary>
        /// </summary>
        /// <param name="sender">メッセージ送信元。</param>
        public DistributiveChannel(ISender<Holder<TMessage>> sender)
        {
            _inner = sender;
        }

        /// <summary><see cref="ISender{TMessage}"/></summary>
        public IDisposable Subscribe(AsyncAction<DistributiveMessage<TMessage>> handler)
            => _inner.Subscribe(async (message, ct) =>
            {
                var array = message.Array;
                if (array != null)
                {
                    await Task.WhenAll(array.Select(x => HandleAsync(handler, x, ct)));
                }
                else
                {
                    await HandleAsync(handler, message.Value, ct);
                }
            });

        private Task HandleAsync(AsyncAction<DistributiveMessage<TMessage>> handler, TMessage x, System.Threading.CancellationToken ct)
        {
            var responsive = x as IResponsiveMessage;
            return handler(new DistributiveMessage<TMessage>(responsive?.Address, x), ct);
        }

        /// <summary><see cref="ISender{TMessage}"/></summary>
        public Task Completed => _inner.Completed;

        /// <summary>
        /// 指定した配送先のメッセージだけを取り出すチャネルを取得。
        /// </summary>
        /// <param name="address">配送先。全体宛の(配送先の指定がない)メッセージを受け取りたい場合はnullを渡す。</param>
        /// <returns></returns>
        public ISender<TMessage> GetChannel(int? address)
            => this.Filter(x => x.Address == address, x => x.Message);
    }

    public static partial class Channel
    {
        /// <summary>
        /// 拡張メソッドで<see cref="DistributiveChannel{TMessage}"/>生成。
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="sender"></param>
        /// <returns></returns>
        public static DistributiveChannel<TMessage> Distribute<TMessage>(this ISender<Holder<TMessage>> sender)
            => new DistributiveChannel<TMessage>(sender);
    }
}
