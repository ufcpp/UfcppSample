using System;

namespace WeakReferenceSample.WeakEvent
{
    public static partial class WeakEventExtensions
    {
        /// <summary>
        /// 【参考実装】デストラクターを使う版
        ///
        /// 弱イベント購読。
        /// 戻り値の <see cref="IDisposable"/> が誰からも参照されなくなったら自動的にイベント購読解除する。
        /// </summary>
        /// <remarks>
        /// コードはものすごくシンプルになってうれしいんだけども…
        /// デストラクターは結構高コストな存在なので、実行性能的には弱参照使う方がいいはず。
        /// </remarks>
        public static IDisposable FinalizeSubscribe<T>(this IObservable<T> observable, Action<T> onNext)
        {
            var subscription = observable.Subscribe(onNext);
            return new FinalizeDisposable(subscription);
        }
    }

    /// <summary>
    /// デストラクター(Finalize)でも <see cref="IDisposable.Dispose"/> が呼ばれるようにする。
    /// </summary>
    class FinalizeDisposable : IDisposable
    {
        IDisposable _inner;

        public FinalizeDisposable(IDisposable inner) { _inner = inner; }

        ~FinalizeDisposable()
        {
            _inner.Dispose();
        }

        public void Dispose()
        {
            _inner.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
