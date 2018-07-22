using System;
using System.Diagnostics;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;

namespace CachedAsync
{
    /// <summary>
    /// <see cref="AsyncOperation{TResult}"/>から使う静的メンバー。
    /// 全ての型引数に対して共通のインスタンスを使いたいので、分離。
    /// 元々は protected にして、<see cref="AsyncOperation{TResult}"/> が <see cref="AsyncOperation"/> を継承してた。
    /// 元は <see cref="AsyncOperation{TResult}"/> の方も internal だったからそれでよかったけど、public にするには不都合なので。
    /// </summary>
    internal class AsyncOperation
    {
        /// <summary>いわゆる「番兵」。1インスタンスを使いまわす想定なので、「使われてない」を表す状態が必要。continuation にこの Action インスタンスを入れることで表現。</summary>
        internal static readonly Action<object> s_availableSentinel = new Action<object>(s => Debug.Fail($"{nameof(AsyncOperation)}.{nameof(s_availableSentinel)} invoked with {s}."));
        /// <summary>こっちは「完了済み」を表す番兵。</summary>
        internal static readonly Action<object> s_completedSentinel = new Action<object>(s => Debug.Fail($"{nameof(AsyncOperation)}.{nameof(s_completedSentinel)} invoked with {s}"));

        internal static void ThrowIncompleteOperationException() =>
            throw new InvalidOperationException("the operation's result was accessed before the operation completed.");

        internal static void ThrowMultipleContinuations() =>
            throw new InvalidOperationException("multiple continuations can't be set for the same operation.");

        internal static void ThrowIncorrectCurrentIdException() =>
            throw new InvalidOperationException("the operation was used after it was supposed to be used.");
    }

    /// <summary>
    /// https://github.com/dotnet/corefx/blob/master/src/System.Threading.Channels/src/System/Threading/Channels/AsyncOperation.cs から、
    /// デモに必要な最低限だけ残して削ったもの。
    ///
    /// - 常にプール前提
    /// - <see cref="ExecutionContext"/>、<see cref="SynchronizationContext"/> を使わない
    /// - コンストラクターで <see cref="CancellationToken"/> を渡してキャンセルする口をなくした
    /// - 完了処理のとき、<see cref="TaskFactory.StartNew(Action{object}, object)"/> せずに直呼び
    /// - 型引数なしの <see cref="IValueTaskSource"/> は削った
    ///
    /// あと、コメントの和訳。
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    public class AsyncOperation<TResult> : IValueTaskSource<TResult>
    {
        private TResult _result;
        private ExceptionDispatchInfo _error;
        private Action<object> _continuation = AsyncOperation.s_availableSentinel;
        private object _continuationState;

        /// <summary>現在の非同期操作に紐づいてる値。</summary>
        /// <remarks>
        /// <see cref="IValueTaskSource{TResult}.GetResult(short)"/>
        /// キャッシュしてるインスタンスが想定外の使われ方すると token がずれるはずなので、それの検出用。
        /// GetResult が呼ばれるたびにインクリメントする。
        /// </remarks>
        private short _currentId;

        /// <summary>このインスタンスと現在の token から作った <see cref="ValueTask{TResult}"/> を返す。</summary>
        public ValueTask<TResult> Task => new ValueTask<TResult>(this, _currentId);

        /// <summary>非同期操作の現在の状態を取得。</summary>
        /// <param name="token"><see cref="_currentId"/> と一致している必要あり。</param>
        ValueTaskSourceStatus IValueTaskSource<TResult>.GetStatus(short token)
        {
            if (_currentId == token)
            {
                return
                    !IsCompleted ? ValueTaskSourceStatus.Pending :
                    _error == null ? ValueTaskSourceStatus.Succeeded :
                    _error.SourceException is OperationCanceledException ? ValueTaskSourceStatus.Canceled :
                    ValueTaskSourceStatus.Faulted;
            }

            AsyncOperation.ThrowIncorrectCurrentIdException();
            return default; //↑ が常に throw してるってのはコンパイラーにはわからないので、追加で return。
        }

        /// <summary>非同期操作が完了しているかを取得。</summary>
        internal bool IsCompleted => ReferenceEquals(_continuation, AsyncOperation.s_completedSentinel);

        /// <summary>非同期操作の結果を取得。</summary>
        /// <param name="token"><see cref="_currentId"/> と一致している必要あり。</param>
        TResult IValueTaskSource<TResult>.GetResult(short token)
        {
            if (_currentId != token)
            {
                AsyncOperation.ThrowIncorrectCurrentIdException();
            }

            if (!IsCompleted)
            {
                AsyncOperation.ThrowIncompleteOperationException();
            }

            ExceptionDispatchInfo error = _error;
            TResult result = _result;
            _currentId++;

            Volatile.Write(ref _continuation, AsyncOperation.s_availableSentinel); // result とかの他のデータをフェッチしてから出ないとダメ

            error?.Throw();
            return result;
        }

        /// <summary>キャッシュしている値の所有権取得を試みる。</summary>
        /// <returns>呼び出し元が所有権を取れたら true。この場合、ステートを取得する。取れなかったら false。</returns>
        public bool TryOwnAndReset()
        {
            if (ReferenceEquals(Interlocked.CompareExchange(ref _continuation, null, AsyncOperation.s_availableSentinel), AsyncOperation.s_availableSentinel))
            {
                _continuationState = null;
                _result = default;
                _error = null;
                return true;
            }

            return false;
        }

        /// <summary>非同期操作の完了時に読んでもらうコールバックを登録する。</summary>
        /// <remarks>
        /// すでに完了済みの状態で呼ばれた場合、
        /// 元々の実装では <see cref="TaskFactory.StartNew(Action{object}, object)"/> してた。
        /// この実装は単に同期で continuation(state) 呼び出し。
        /// オーバーヘッドは掛からないけども、スタックが深くなるので注意。
        /// </remarks>
        void IValueTaskSource<TResult>.OnCompleted(Action<object> continuation, object state, short token, ValueTaskSourceOnCompletedFlags flags)
        {
            if (_currentId != token)
            {
                AsyncOperation.ThrowIncorrectCurrentIdException();
            }

            // CompareExchange 直後に完了している場合に備えて、CompareExchange より前にステートを書き込まないとダメ。
            // こちらの意図に反して誰かが複数の continuation を間違えてスケジューリングしようとした場合、間違ったステートを使ってしまうことになる。
            // できる限りはそういう誤用を拾いたい。
            if (_continuationState != null)
            {
                AsyncOperation.ThrowMultipleContinuations();
            }
            _continuationState = state;

            // 引数の continuation をフィールド _continuation にセットするのを試みる。
            // 成功は、非同期操作がまだ未完了ということになる。completer (完了処理の債務を負ってる人)は後々コールバックを呼ぶ責任がある。
            // 失敗は、非同期操作がすでに完了していることになる。ここで continuation を呼び出す必要がある。
            //
            // ただ、ここは awaiter の OnCompleted の中から呼ばれているはずで、スタックが詰まれるのを避けたい。
            // なので、(元々の実装では)continuation は非同期に読ぶ。
            Action<object> prevContinuation = Interlocked.CompareExchange(ref _continuation, continuation, null);
            if (prevContinuation != null)
            {
                // ここに入った時点で完了済みのはずだけど、何か誤用があった場合には想定外の状態になってることがあるので確認。
                Debug.Assert(IsCompleted, $"Expected IsCompleted");
                if (!ReferenceEquals(prevContinuation, AsyncOperation.s_completedSentinel))
                {
                    Debug.Assert(prevContinuation != AsyncOperation.s_availableSentinel, "Continuation was the available sentinel.");
                    AsyncOperation.ThrowMultipleContinuations();
                }

                //// 元の実装だと StartNew してスレッドプールに enqueue。
                //Task.Factory.StartNew(continuation, state, CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);
                continuation(state);
            }
        }

        /// <summary>非同期操作を完了させる(成功時)</summary>
        /// <returns>この実装だと常に true に。 SetResult に変更してもよかったかも。</returns>
        public bool TrySetResult(TResult item)
        {
            _result = item;
            SignalCompletion();
            return true;
        }

        /// <summary>非同期操作を完了させる(失敗時)</summary>
        /// <returns>この実装だと常に true に。 SetException に変更してもよかったかも。</returns>
        public bool TrySetException(Exception exception)
        {
            _error = ExceptionDispatchInfo.Capture(exception);
            SignalCompletion();
            return true;
        }

        /// <summary>非同期操作を完了させる(キャンセル時)</summary>
        /// <returns>この実装だと常に true に。 SetCancel に変更してもよかったかも。</returns>
        public bool TrySetCanceled(CancellationToken cancellationToken = default)
        {
            _error = ExceptionDispatchInfo.Capture(new OperationCanceledException(cancellationToken));
            SignalCompletion();
            return true;
        }

        /// <summary>登録された continuation を呼び出す。</summary>
        private void SignalCompletion()
        {
            if (_continuation != null || Interlocked.CompareExchange(ref _continuation, AsyncOperation.s_completedSentinel, null) != null)
            {
                Debug.Assert(_continuation != AsyncOperation.s_completedSentinel, $"The continuation was the completion sentinel.");
                Debug.Assert(_continuation != AsyncOperation.s_availableSentinel, $"The continuation was the available sentinel.");

                Action<object> c = _continuation;
                _continuation = AsyncOperation.s_completedSentinel;
                c(_continuationState);
            }
        }
    }
}

