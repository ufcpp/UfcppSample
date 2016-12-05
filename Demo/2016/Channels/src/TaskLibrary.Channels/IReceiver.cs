using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TaskLibrary.Channels
{
    /// <summary>
    /// イベントを受け取る側のインターフェイス。
    /// </summary>
    /// <typeparam name="TMessage">イベント引数の型。</typeparam>
    /// <remarks>
    /// チャネルを閉じるときに<see cref="IDisposable.Dispose"/>を呼ぶ。
    /// </remarks>
    public interface IReceiver<TMessage>
    {
        /// <summary>
        /// イベントを通知する。
        /// </summary>
        /// <param name="message">イベント引数。</param>
        /// <param name="ct">ハンドラーの処理を止めるためのトークン。</param>
        /// <returns>ハンドラー側の処理が終わるのを待つ。</returns>
        Task OnNext(Holder<TMessage> message, CancellationToken ct);

        /// <summary>
        /// このreceiverを使っている非同期メソッドの完了待ちをするタスクを設定する。
        /// </summary>
        /// <param name="completion"></param>
        /// <remarks>
        /// 最初は、
        /// - チャネルを使い終わったらDisposeを呼んでもらう
        /// - チャネル内で <see cref="TaskCompletionSource{TResult}.TrySetResult(TResult)"/> 呼んで <see cref="ISender{TMessage}.Completed"/> を完了状態にする
        /// みたいな構造だったけど、それだと await して例外拾うとかできないんで。
        ///
        /// receiverを使っている非同期メソッド自体をsetする構造にした方が便利そうだった。
        /// </remarks>
        void SetCompletion(Task completion);
    }

    /// <summary>
    /// <see cref="IReceiver{TMessage}"/>向け拡張。
    /// </summary>
    public static class ReceiverExtensions
    {
        /// <summary>
        /// イベントを送って、ハンドラーの処理が終わるのを待つ。
        /// </summary>
        /// <typeparam name="TMessageBase"><see cref="IReceiver{TMessage}"/>に渡す、メッセージの基底型。</typeparam>
        /// <typeparam name="TMessage">実際に送りたいメッセージの型。</typeparam>
        /// <param name="reciever">メッセージの送り先。</param>
        /// <param name="message"><see cref="IReceiver{TMessage}"/></param>
        /// <param name="ct"><see cref="IReceiver{TMessage}"/></param>
        /// <returns>引数を素通し。</returns>
        /// <remarks>
        /// 基本的には<see cref="IReceiver{TMessage}.OnNext(Holder{TMessage}, CancellationToken)"/>呼んでるだけ。
        /// <see cref="IResponsiveMessage{T}.Response"/>を抜き出すためとかに正確な型が必要なので、型引数<typeparamref name="TMessage"/>を用意したり、戻り値に引数を素通りさせたりしてる。
        ///
        /// <see cref="ResponsiveMessageExtensions.GetResponse{TResponse}(IResponsiveMessage{TResponse})"/>と組み合わせて使う。
        /// <![CDATA[ TResponse res = (await receiver.SendAsync(message, ct)).GetResponse(); ]]>
        /// </remarks>
        public static async Task<TMessage> SendAsync<TMessageBase, TMessage>(this IReceiver<TMessageBase> reciever, TMessage message, CancellationToken ct)
            where TMessage : TMessageBase
        {
            await reciever.OnNext(message, ct);
            return message;
        }

        /// <summary>
        /// イベントを送って、ハンドラーの処理が終わるのを待つ。
        /// 並列メッセージ送付版。
        /// </summary>
        /// <typeparam name="TMessageBase"><see cref="IReceiver{TMessage}"/>に渡す、メッセージの基底型。</typeparam>
        /// <typeparam name="TMessage">実際に送りたいメッセージの型。</typeparam>
        /// <param name="reciever">メッセージの送り先。</param>
        /// <param name="messages"><see cref="IReceiver{TMessage}"/></param>
        /// <param name="ct"><see cref="IReceiver{TMessage}"/></param>
        /// <returns>引数を素通し。</returns>
        /// <remarks>
        /// <see cref="SendAsync{TMessageBase, TMessage}(IReceiver{TMessageBase}, TMessage, CancellationToken)"/>。
        /// 加えて、<typeparamref name="TMessage"/>の配列から<typeparamref name="TMessageBase"/>の配列への変換とか。
        ///
        /// <see cref="ResponsiveMessageExtensions.GetResponse{TResponse}(IEnumerable{IResponsiveMessage{TResponse}})"/>と組み合わせて使う。
        /// <![CDATA[ IEnumerable<TResponse> res = (await receiver.SendAsync(messages, ct)).GetResponse(); ]]>
        ///
        /// <see cref="Holder{T}"/>を挟んじゃってるせいで共変性が効かないので、配列のインスタンスが無駄に1個作られてたりはする。
        /// </remarks>
        public static async Task<IEnumerable<TMessage>> SendAsync<TMessageBase, TMessage>(this IReceiver<TMessageBase> reciever, IEnumerable<TMessage> messages, CancellationToken ct)
            where TMessage : TMessageBase
        {
            await reciever.OnNext(messages.Cast<TMessageBase>().ToArray(), ct);
            return messages;
        }
    }
}
