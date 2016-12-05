using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SystemAsync;

namespace TaskLibrary.Channels
{
    /// <summary>
    /// 再現実行かどうかのフラグ ＋ メッセージ。
    /// </summary>
    /// <typeparam name="TMessage">メッセージ(イベント引数)の型。</typeparam>
    public struct ReplicableMessage<TMessage>
    {
        /// <summary>
        /// 再現実行で自動的に応答が入っている状態ならtrue。
        /// </summary>
        public bool IsAuto { get; }

        /// <summary>
        /// 何番目のメッセージか。
        /// </summary>
        public int SequenceNumber { get; }

        /// <summary>
        /// 元々のメッセージ。
        /// </summary>
        public Holder<TMessage> Message { get; }

        /// <summary></summary>
        /// <param name="isAuto"><see cref="IsAuto"/></param>
        /// <param name="sequenceNumber"><see cref="SequenceNumber"/></param>
        /// <param name="message"><see cref="Message"/></param>
        public ReplicableMessage(bool isAuto, int sequenceNumber, Holder<TMessage> message)
        {
            IsAuto = isAuto;
            SequenceNumber = sequenceNumber;
            Message = message;
        }
    }

    /// <summary>
    /// 応答の記録付きのチャネル。
    /// </summary>
    /// <typeparam name="TMessage">メッセージ(イベント引数)の型。</typeparam>
    /// <typeparam name="TResponse">応答の型。</typeparam>
    /// <remarks>
    /// チャネルのメッセージ送信側は、(同じ乱数シードを与えれば)必ず同じ順でメッセージを送ってくるものとする。
    /// この場合、応答を記録・再生すれば、完全に状況再現(replication)ができるはず。
    /// その、記録と再生を担うクラス。
    ///
    /// 他のチャネルはSubscribe時にinner.Subscribeを呼ぶ作りなんだけど、こいつだけはコンストラクターでinner.Subscribeしてる。
    /// (なので、このクラスの Dispose をちゃんと呼ばないと購読解除できない。)
    /// こういう作りでないと、複数のハンドラーを登録したときにMoveNextが多重に呼ばれて挙動狂う。
    /// このクラスは複数のハンドラーを登録するのには向いてないんで、そもそもそれを禁止した方がいいかも。
    /// </remarks>
    public class ReplicableChannel<TMessage, TResponse> : ISender<ReplicableMessage<TMessage>>, IDisposable
    {
        private readonly IDisposable _subscription;
        private readonly ISender<Holder<TMessage>> _inner;
        private readonly List<RecordedResponse<TResponse>> _responses = new List<RecordedResponse<TResponse>>();

        /// <summary>
        /// </summary>
        /// <param name="sender">メッセージの送信元。</param>
        /// <param name="initialResponses">再生したい応答。途中まで(メッセージ数に応答数が合わない・足りない)でもOK。その場合、再生中はisAutoがtrue、それ以降はisAutoがfalseになる。</param>
        /// <param name="mode"><see cref="InvocationMode"/></param>
        public ReplicableChannel(ISender<Holder<TMessage>> sender, IEnumerable<TResponse> initialResponses = null, InvocationMode mode = InvocationMode.Parallel)
        {
            var e = initialResponses?.ToArray().GetEnumerator();
            _inner = sender;
            int sequenceNumber = 0;
            _subscription = sender.Subscribe(async (message, ct) =>
            {
                sequenceNumber++;

                // 再現実行用。
                var isAuto = false;
                var array = message.Array;
                if (array != null)
                {
                    foreach (var x in array)
                    {
                        Replicate(ref e, ref isAuto, x);
                    }
                }
                else
                {
                    var x = message.Value;
                    Replicate(ref e, ref isAuto, x);
                }

                await _handlers.InvokeAsync(mode, new ReplicableMessage<TMessage>(isAuto, sequenceNumber, message), ct);

                // 結果記憶用。
                if (array != null)
                {
                    foreach (var x in array)
                    {
                        var responsive = x as IResponsiveMessage;
                        if (responsive != null)
                            _responses.Add(new RecordedResponse<TResponse>(sequenceNumber, responsive.Address, (TResponse)responsive.Response));
                    }
                }
                else
                {
                    var x = message.Value;
                    var responsive = x as IResponsiveMessage;
                    if (responsive != null)
                        _responses.Add(new RecordedResponse<TResponse>(sequenceNumber, responsive.Address, (TResponse)responsive.Response));
                }
            });
        }

        /// <summary>
        /// 記録した応答一覧。
        /// </summary>
        public IEnumerable<RecordedResponse<TResponse>> Responses => _responses;

        /// <summary>
        /// 再現実行状態のものは除いて、手動実行が必要なメッセージだけを通すチャネル。
        /// </summary>
        public ISender<Holder<TMessage>> ManualChannel => this.Filter(x => !x.IsAuto, x => x.Message);

        private static void Replicate(ref System.Collections.IEnumerator e, ref bool isAuto, TMessage x)
        {
            var responsive = x as IResponsiveMessage;

            if (e != null && responsive != null)
            {
                if (e.MoveNext())
                {
                    responsive.Response = e.Current;
                    isAuto = true;
                }
                else e = null;
            }
        }

        private AsyncActionList<ReplicableMessage<TMessage>> _handlers;

        /// <summary><see cref="ISender{TMessage}"/></summary>
        public IDisposable Subscribe(AsyncAction<ReplicableMessage<TMessage>> handler)
        {
            _handlers.Add(handler);
            return Disposable.Create(() => _handlers.Remove(handler));
        }

        /// <summary><see cref="ISender{TMessage}"/></summary>
        public Task Completed => _inner.Completed;

        /// <summary>購読解除</summary>
        public void Dispose() => _subscription.Dispose();
    }
}
