namespace Observable
{
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    static class ParameterProbrem
    {
        /// <summary>
        /// ボタンのクリックを1回受け取るまで待ちたい
        /// </summary>
        public static Task FirstClickAsync(this Button x)
        {
            var tcs = new TaskCompletionSource<bool>();
            RoutedEventHandler handler = null;
            handler = (sender, arg) =>
            {
                x.Click -= handler;
                tcs.TrySetResult(false);
            };
            x.Click += handler;
            return tcs.Task;
        }


        /// <summary>
        /// ボタンのダブルクリックを1回受け取るまで待ちたい
        /// FirstClickAsync とほとんど同じなのに…
        /// </summary>
        public static Task FirstDoubleClickAsync(this Button x)
        {
            var tcs = new TaskCompletionSource<bool>();
            MouseButtonEventHandler handler = null;
            handler = (sender, arg) =>
            {
                x.MouseDoubleClick -= handler;
                tcs.TrySetResult(false);
            };
            x.MouseDoubleClick += handler;
            return tcs.Task;
        }

#if false
        /// <summary>
        /// こんな感じのコードが書けたらよかったのにな(もちろん無理。コンパイル エラー)
        /// </summary>
        public static Task FirstAsync<TEventHandler>(this event TEventHandler x)
        {
            var tcs = new TaskCompletionSource<bool>();
            TEventHandler handler = null;
            handler = (sender, arg) =>
            {
                x -= handler;
                tcs.TrySetResult(false);
            };
            x += handler;
            return tcs.Task;
        }
#endif
    }
}

namespace Observable
{
    using System;
    using System.Reactive.Disposables;

    /// <summary>
    /// イベントの登録口。
    /// </summary>
    public interface IEvent<TArg>
    {
        IDisposable Subscribe(EventHandler<TArg> handler);
    }

    /// <summary>
    /// イベントの登録口の実装 + イベントを起こす機能。
    /// </summary>
    public class Event<TArg> : IEvent<TArg>
    {
        event EventHandler<TArg> e;

        /// <summary>
        /// イベント登録。
        /// </summary>
        public IDisposable Subscribe(EventHandler<TArg> handler)
        {
            e += handler;
            return Disposable.Create(() => e -= handler);
        }

        /// <summary>
        /// イベントを起こす。
        /// </summary>
        public void Raise(object sender, TArg arg) => e?.Invoke(sender, arg);
    }
}

namespace Observable
{
    using System;
    using System.Threading.Tasks;

    public static class EventExtensions
    {
        /// <summary>
        /// イベントを1回受け取るまで待ちたい
        /// </summary>
        /// <typeparam name="TArg"></typeparam>
        /// <param name="x"></param>
        /// <returns></returns>
        public static Task FirstAsync<TArg>(this IEvent<TArg> x)
        {
            var tcs = new TaskCompletionSource<bool>();
            IDisposable subscription = null;
            subscription = x.Subscribe((sender, arg) =>
            {
                subscription.Dispose();
                tcs.TrySetResult(false);
            });
            return tcs.Task;
        }
    }
}

namespace Observable
{
    using System.Windows;
    using System.Windows.Input;

    /// <summary>
    /// <see cref="Button"/> が、というか、event 構文がこうだったらよかったのに。
    /// という例。
    /// 1段階オブジェクトが挟まってしまうのがいやだけども、使い勝手は上がる。
    /// </summary>
    class PreferredButton
    {
        public IEvent<RoutedEventArgs> Click => _click;
        private Event<RoutedEventArgs> _click = new Event<RoutedEventArgs>();

        public IEvent<MouseButtonEventArgs> MouseDoubleClick => _mouseDoubleClick;
        private Event<MouseButtonEventArgs> _mouseDoubleClick = new Event<MouseButtonEventArgs>();
    }
}
