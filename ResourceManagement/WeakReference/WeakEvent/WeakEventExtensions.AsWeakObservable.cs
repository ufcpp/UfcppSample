using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace WeakReferenceSample.WeakEvent
{
    public static partial class WeakEventExtensions
    {
        /// <summary>
        /// 【参考実装】Subscribe 時じゃなくて、IObservable に対して細工する版。
        ///
        /// <paramref name="observable"/> をラップして、Subscribe が全部弱イベント購読になる <see cref="IObservable{T}"/> を作る。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="observable"></param>
        /// <returns></returns>
        public static IObservable<T> AsWeakObservable<T>(this IObservable<T> observable)
        {
            return Observable.Create((IObserver<T> observer) =>
            {
                WeakReference<IDisposable> weakSubscription = null;
                IDisposable subscription = null;

                subscription = observable.Subscribe(x =>
                {
                    IDisposable d;
                    if (!weakSubscription.TryGetTarget(out d))
                    {
                        subscription.Dispose();
                        return;
                    }
                    observer.OnNext(x);
                });

                var s = new SingleAssignmentDisposable();
                s.Disposable = subscription;

                weakSubscription = new WeakReference<IDisposable>(s);
                return s;
            });
        }
    }
}
