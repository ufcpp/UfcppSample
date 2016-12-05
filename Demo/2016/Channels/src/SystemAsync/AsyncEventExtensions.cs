using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SystemAsync
{
    /// <summary>
    /// <see cref="IAsyncEvent{TEventArgs}"/>拡張。
    /// </summary>
    public static class AsyncEventExtensions
    {
        /// <summary>
        /// <see cref="AsyncAction{T1}"/>以外の形式でイベントを購読する。
        /// </summary>
        public static IDisposable Subscribe<T>(this IAsyncEvent<T> e, Func<T, Task> handler) => e.Subscribe((_1, arg) => handler(arg));

        /// <summary>
        /// <see cref="AsyncAction{T1}"/>以外の形式でイベントを購読する。
        /// </summary>
        public static IDisposable Subscribe<T>(this IAsyncEvent<T> e, Func<Task> handler) => e.Subscribe((_1, _2) => handler());

        /// <summary>
        /// <see cref="AsyncAction{T1}"/>以外の形式でイベントを購読する。
        /// </summary>
        public static IDisposable Subscribe<T>(this IAsyncEvent<T> e, Action handler) => e.Subscribe((_1, _2) => { handler(); return Task.CompletedTask; });

        /// <summary>
        /// <see cref="AsyncAction{T1}"/>以外の形式でイベントを購読する。
        /// </summary>
        public static IDisposable Subscribe<T>(this IAsyncEvent<T> e, Action<T> handler) => e.Subscribe((_1, args) => { handler(args); return Task.CompletedTask; });

        /// <summary>
        /// キャンセルされるまでの間イベントを購読する。
        /// </summary>
        public static void SubscribeUntil<T>(this IAsyncEvent<T> e, CancellationToken ct, AsyncHandler<T> handler)
        {
            var d = e.Subscribe(handler);
            ct.Register(d.Dispose);
        }

        /// <summary>
        /// キャンセルされるまでの間イベントを購読する。
        /// </summary>
        public static void SubscribeUntil<T>(this IAsyncEvent<T> e, CancellationToken ct, Func<T, Task> handler)
        {
            var d = e.Subscribe(handler);
            ct.Register(d.Dispose);
        }

        /// <summary>
        /// キャンセルされるまでの間イベントを購読する。
        /// </summary>
        public static void SubscribeUntil<T>(this IAsyncEvent<T> e, CancellationToken ct, Func<Task> handler)
        {
            var d = e.Subscribe(handler);
            ct.Register(d.Dispose);
        }
    }
}
