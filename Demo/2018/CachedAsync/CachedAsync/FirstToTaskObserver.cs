using System;
using System.Threading;

namespace CachedAsync
{
    /// <summary>
    /// observable.FirstAsync(predicate).Select(selector).ToTask(ct) 相当の処理をオーバーヘッド少な目で行うために、
    /// <see cref="IObserver{T}"/> かつ <see cref="AsyncOperation{TResult}"/> な型を用意。
    /// </summary>
    /// <remarks>
    /// ・使い方
    /// First().ToTask()/ToFirstTask() してたところを、new FirstToTaskObservable(observable, ct) に置き換えればOK。
    ///
    /// ・ManualResetValueTaskSource
    /// 参考: https://github.com/dotnet/corefx/blob/master/src/Common/tests/System/Threading/Tasks/Sources/ManualResetValueTaskSource.cs
    ///
    /// <see cref="AsyncOperation{TResult}"/> は↑この型がなかった頃に書いたもので、このクラスも同時期に書いたもの。
    /// 今から作るなら普通にこの ManualResetValueTaskSource を使うと思う。
    /// </remarks>
    public abstract class FirstToTaskOpeartion<TArg, TResult> : AsyncOperation<TResult>, IObserver<TArg>
    {
        private IDisposable _subscription;
        private CancellationTokenRegistration _ctr;
        private readonly bool _noThrowOnCancel;

        /// <summary>
        /// </summary>
        /// <param name="observable">イベント発生元。</param>
        /// <param name="ct">キャンセル用。</param>
        /// <param name="noThrowOnCancel">キャンセル時に TrySetCancel (例外誘発)するか、TrySetResult(defalt) するか。</param>
        public FirstToTaskOpeartion(IObservable<TArg> observable, CancellationToken ct = default, bool noThrowOnCancel = false)
        {
            TryOwnAndReset();
            _subscription = observable.Subscribe(this);
            _ctr = ct.Register(Cancel);
            _noThrowOnCancel = noThrowOnCancel;
        }

        /// <summary>
        /// キャンセル処理。
        /// </summary>
        protected virtual void Cancel()
        {
            _subscription.Dispose();
            _ctr.Dispose();

            if (_noThrowOnCancel) TrySetCanceled();
            else TrySetResult(default);
        }

        void IObserver<TArg>.OnCompleted()
        {
            Cancel();
        }

        void IObserver<TArg>.OnError(Exception error)
        {
            _subscription.Dispose();
            _ctr.Dispose();
            TrySetException(error);
        }

        void IObserver<TArg>.OnNext(TArg value)
        {
            try
            {
                if (TryGetResult(value, out var result, out var exception))
                {
                    _subscription.Dispose();
                    _ctr.Dispose();

                    if (exception == null) TrySetResult(result);
                    else TrySetException(exception);
                }
            }
            catch (Exception ex)
            {
                TrySetException(ex);
            }
        }

        /// <summary>
        /// observable.FirstAsync(predicate).Select(selector).ToTask(ct)
        /// の selector 部分。
        /// <see cref="IObservable{T}"/> 側の T を <see cref="AsyncOperation{TResult}"/> 側の TResult に変換。
        /// </summary>
        /// <returns>
        /// 戻り値が true の時には <see cref="AsyncOperation{TResult}.TrySetResult(TResult)"/> とかを呼んでタスクを完了する。
        /// false の時はまだ Subscribe を続ける。
        /// </returns>
        protected abstract bool TryGetResult(TArg arg, out TResult result, out Exception exception);
    }

    /// <summary>
    /// <see cref="FirstToTaskOpeartion{TArg, TResult}"/>
    /// 無変換版。
    /// </summary>
    public class FirstToTaskOpeartion<TResult> : FirstToTaskOpeartion<TResult, TResult>
    {
        /// <summary>
        /// <see cref="FirstToTaskOpeartion{TArg, TResult}"/>
        /// </summary>
        public FirstToTaskOpeartion(IObservable<TResult> observable, CancellationToken ct = default, bool noThrowOnCancel = false)
            : base(observable, ct, noThrowOnCancel)
        {
        }

        /// <summary><see cref="FirstToTaskOpeartion{TArg, TResult}"/></summary>
        protected override bool TryGetResult(TResult arg, out TResult result, out Exception exception)
        {
            result = arg;
            exception = null;
            return true;
        }
    }

    /// <summary>
    /// デリゲート実装の <see cref="FirstToTaskOpeartion{TArg, TResult}"/>。
    /// </summary>
    /// <remarks>
    /// <see cref="FirstToTaskOpeartion{TArg, TResult}"/>は、
    /// デリゲートのインスタンス分のアロケーションも避けたいっていう要求から作ったものなので、
    /// このクラスは存在自体がちょっと矛盾してるんだけど。
    /// 一応、手軽さが欲しいとき用に作るだけは作る。
    /// </remarks>
    public class DelegateFirstToTaskOpeartion<TArg, TResult> : FirstToTaskOpeartion<TArg, TResult>
    {
        private readonly Func<TArg, bool> _predicate;
        private readonly Func<TArg, TResult> _selector;

        /// <summary><see cref="FirstToTaskOpeartion{TArg, TResult}"/></summary>
        public DelegateFirstToTaskOpeartion(IObservable<TArg> observable, Func<TArg, bool> predicate, Func<TArg, TResult> selector, CancellationToken ct = default, bool noThrowOnCancel = false)
            : base(observable, ct, noThrowOnCancel)
        {
            _predicate = predicate ?? throw new NullReferenceException(nameof(predicate));
            _selector = selector ?? throw new NullReferenceException(nameof(selector));
        }

        /// <summary><see cref="FirstToTaskObservable{TArg, TResult}"/></summary>
        protected override bool TryGetResult(TArg arg, out TResult result, out Exception exception)
        {
            if (_predicate(arg))
            {
                try
                {
                    result = _selector(arg);
                    exception = null;
                }
                catch (Exception ex)
                {
                    result = default;
                    exception = ex;
                }
                return true;
            }
            else
            {
                exception = null;
                result = default;
                return false;
            }
        }
    }
}
