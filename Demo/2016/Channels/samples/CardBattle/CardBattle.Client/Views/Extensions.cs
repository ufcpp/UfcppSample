using System;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace CardBattle.Views
{
    static class Extensions
    {
        /// <summary>
        /// ボタンが1回押されるのを待つタスクを作る。
        /// </summary>
        /// <param name="b"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public static Task FirstClick(this Button b, CancellationToken ct)
            => Observable.FromEventPattern<RoutedEventHandler, RoutedEventArgs>(
                h => b.Click += h,
                h => b.Click -= h)
            .FirstAsync()
            .ToTask(ct);

        /// <summary>
        /// ストーリーボードの再生。
        /// </summary>
        /// <param name="storyboard"></param>
        /// <returns>再生完了待ちタスク。</returns>
        public static Task PlayAsync(this Storyboard storyboard)
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            if (storyboard == null)
                tcs.SetException(new ArgumentNullException());
            else
            {
                EventHandler onComplete = null;
                onComplete = (s, e) => {
                    storyboard.Completed -= onComplete;
                    tcs.SetResult(true);
                };
                storyboard.Completed += onComplete;
                storyboard.Begin();
            }
            return tcs.Task;
        }
    }
}
