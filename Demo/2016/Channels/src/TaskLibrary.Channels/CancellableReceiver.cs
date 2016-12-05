using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TaskLibrary.Channels
{
    /// <summary>
    /// (<see cref="Channel{TMessage}"/>, <see cref="CancellationToken"/>)のペア。
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public struct CancellableReceiver<TMessage>
    {
        /// <summary>
        /// チャネル。
        /// </summary>
        public IReceiver<TMessage> Receiver { get; }

        /// <summary>
        /// <see cref="IReceiver{TMessage}.OnNext(Holder{TMessage}, CancellationToken)"/>に渡すトークン。
        /// </summary>
        public CancellationToken CancellationToken { get; }

        /// <summary></summary>
        /// <param name="receiver"><see cref="Channel"/></param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        public CancellableReceiver(IReceiver<TMessage> receiver, CancellationToken cancellationToken)
        {
            Receiver = receiver;
            CancellationToken = cancellationToken;
        }
    }

    public static class CancellableChannel
    {
        /// <summary>
        /// <see cref="IReceiver{TMessage}"/>と<see cref="CancellationToken"/>は大体ペアで引き回すことになるので、ペア化してしまう。
        /// </summary>
        public static CancellableReceiver<TMessage> WithCancel<TMessage>(this IReceiver<TMessage> reciever, CancellationToken ct)
            => new CancellableReceiver<TMessage>(reciever, ct);

        /// <summary>
        /// <see cref="CancellableReceiver{TMessage}"/>
        /// <see cref="ReceiverExtensions.SendAsync{TMessageBase, TMessage}(IReceiver{TMessageBase}, TMessage, CancellationToken)"/>
        /// </summary>
        public static Task<TMessage> SendAsync<TMessageBase, TMessage>(this CancellableReceiver<TMessageBase> reciever, TMessage message)
            where TMessage : TMessageBase
            => reciever.Receiver.SendAsync(message, reciever.CancellationToken);

        /// <summary>
        /// <see cref="CancellableReceiver{TMessage}"/>
        /// <see cref="ReceiverExtensions.SendAsync{TMessageBase, TMessage}(IReceiver{TMessageBase}, IEnumerable{TMessage}, CancellationToken)"/>
        /// </summary>
        public static Task<IEnumerable<TMessage>> SendAsync<TMessageBase, TMessage>(this CancellableReceiver<TMessageBase> reciever, TMessage[] messages)
            where TMessage : TMessageBase
            => SendAsync(reciever, (IEnumerable<TMessage>)messages);

        /// <summary>
        /// <see cref="CancellableReceiver{TMessage}"/>
        /// <see cref="ReceiverExtensions.SendAsync{TMessageBase, TMessage}(IReceiver{TMessageBase}, IEnumerable{TMessage}, CancellationToken)"/>
        /// </summary>
        public static Task<IEnumerable<TMessage>> SendAsync<TMessageBase, TMessage>(this CancellableReceiver<TMessageBase> reciever, IEnumerable<TMessage> messages)
            where TMessage : TMessageBase
            => reciever.Receiver.SendAsync(messages, reciever.CancellationToken);

        /// <summary>
        /// 定型処理になりそうだったので静的メソッドでチャネル生成。
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="channel"></param>
        /// <param name="executor">チャネルを使って、メッセージを送り出す非同期メソッド。</param>
        /// <param name="ct">キャンセル用のトークン。</param>
        /// <returns>作ったチャネル。</returns>
        public static void Execute<TMessage>(this IReceiver<TMessage> channel, Func<CancellableReceiver<TMessage>, Task> executor, CancellationToken ct)
        {
            channel.WithCancel(ct).Execute(executor);
        }

        /// <summary>
        /// 定型処理になりそうだったので静的メソッドでチャネル生成。
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="receiver"></param>
        /// <param name="executor">チャネルを使って、メッセージを送り出す非同期メソッド。</param>
        public static void Execute<TMessage>(this CancellableReceiver<TMessage> receiver, Func<CancellableReceiver<TMessage>, Task> executor)
        {
            receiver.Receiver.SetCompletion(executor(receiver));
        }
    }
}
