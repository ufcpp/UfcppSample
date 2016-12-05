using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SystemAsync
{
    /// <summary>
    /// <see cref="AsyncFunc{TResult}"/> 関連拡張。
    /// </summary>
    public static partial class AsyncExtensions
    {

        /// <summary>
        /// 複数のタスクのうち、どれか1つが終わるのを待つ。
        /// その1つのタスクが終わったら、残りのタスクはキャンセルする。
        /// (内部で作った <see cref="CancellationToken"/> を引数に渡してタスクを起動。)
        /// </summary>
        public static Task<T> First<T>(params AsyncFunc<T>[] tasks)
        {
            var cts = new CancellationTokenSource();
            return First<T>(cts, tasks);
        }

        /// <summary>
        /// 複数のタスクのうち、どれか1つが終わるのを待つ。
        /// その1つのタスクが終わったら、残りのタスクはキャンセルする。
        /// (引数で渡した1つの <see cref="CancellationTokenSource.Token"/> を引数に渡してタスクを起動。)
        /// </summary>
        public static Task<T> First<T>(CancellationTokenSource cts, params AsyncFunc<T>[] tasks)
        {
            // JITエラー出るので展開

            if (tasks.Length == 0)
                throw new ArgumentNullException("tasks must contain at least one task", "tasks");

            var t = tasks.Select(x => x(cts.Token)).ToArray();

            var tcs = new TaskCompletionSource<T>();

            foreach (var task in t)
            {
                task.ContinueWith(x =>
                    {
                        if (x.IsCanceled)
                            tcs.TrySetCanceled();
                        else if (x.Exception != null)
                            tcs.TrySetException(x.Exception);
                        else
                            tcs.TrySetResult(x.Result);
                        cts.Cancel();
                    });
            }

            return tcs.Task;
        }

        /// <summary>
        /// 複数のタスクのうち、どれか1つが終わるのを待つ。
        /// その1つのタスクが終わったら、残りのタスクはキャンセルする。
        /// (内部で作った <see cref="CancellationToken"/> を引数に渡してタスクを起動。)
        /// </summary>
        public static Task First(params AsyncAction[] tasks)
        {
            var cts = new CancellationTokenSource();
            return First(cts, tasks);
        }

        /// <summary>
        /// 複数のタスクのうち、どれか1つが終わるのを待つ。
        /// その1つのタスクが終わったら、残りのタスクはキャンセルする。
        /// (引数で渡した1つの <see cref="CancellationTokenSource.Token"/> を引数に渡してタスクを起動。)
        /// </summary>
        public static Task First(CancellationTokenSource cts, params AsyncAction[] tasks)
        {
            if (tasks.Length == 0)
                throw new ArgumentNullException("tasks must contain at least one task", "tasks");

            var t = tasks.Select(x => x(cts.Token)).ToArray();

            var tcs = new TaskCompletionSource<object>();

            foreach (var task in t)
            {
                task.ContinueWith(x =>
                {
                    if (x.Exception != null)
                        tcs.TrySetException(x.Exception);
                    else
                        tcs.TrySetResult(null);
                    cts.Cancel();
                });
            }

            return tcs.Task;
        }
    }
}
