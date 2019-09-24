namespace WeakReferenceSample.WeakEvent
{
    using System;
    using System.Reactive.Disposables;

    public static partial class WeakEventExtensions
    {
        /// <summary>
        /// 弱イベント購読。
        /// 戻り値の <see cref="IDisposable"/> が誰からも参照されなくなったら自動的にイベント購読解除する。
        /// </summary>
        /// <typeparam name="T">イベント引数の型。</typeparam>
        /// <param name="observable">イベント発生側。</param>
        /// <param name="onNext">イベント受取側。</param>
        /// <returns>イベント購読解除用の disposable。</returns>
        /// <remarks>
        /// 弱参照の性質上、<see cref="GC"/> がかかって初めて「誰も使ってない」判定を受ける。
        /// それまではイベント購読解除されず、イベントが届き続ける。
        /// GC タイミングに左右されるコードは推奨できないんで、可能な限り、
        /// 戻り値の <see cref="IDisposable.Dispose"/> を明示的に呼ぶべき。
        /// </remarks>
        public static IDisposable WeakSubscribe<T>(this IObservable<T> observable, Action<T> onNext)
        {
            WeakReference<IDisposable> weakSubscription = null!;
            IDisposable subscription = null!;

            subscription = observable.Subscribe(x =>
            {
                if (!weakSubscription.TryGetTarget(out var d))
                {
                    // 弱参照のターゲットが消えてたらイベント購読解除。
                    subscription.Dispose();
                    return;
                }
                onNext(x);
            });

            // subscription は↑のラムダ式が参照を持っちゃうことになるので、
            // 別の IDisposable を作ってラップ。
            var s = new SingleAssignmentDisposable();
            s.Disposable = subscription;

            // 作った、外から呼ぶ用 IDisposable の弱参照を作る。
            weakSubscription = new WeakReference<IDisposable>(s);
            return s;
        }
    }
}
