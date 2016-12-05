using System;
using System.Threading;
using System.Threading.Tasks;

namespace SystemAsync
{
    /// <summary>
    /// <see cref="SynchronizationContext"/>拡張。
    /// </summary>
    public static class SynchronizationContextExtensions
    {
        /// <summary>
        /// null チェック付き
        /// <paramref name="sync"/> が <see cref="SynchronizationContext.Current"/> と違うときだけ Post。
        /// 同じなら直接アクション呼び出し。
        /// </summary>
        /// <param name="sync"></param>
        /// <param name="action"></param>
        public static void PostIfNotCurrent(this SynchronizationContext sync, Action action)
        {
            if (sync != null && sync != SynchronizationContext.Current)
                sync.Post(x => action(), null);
            else
                action();
        }

        /// <summary>
        /// null チェック付き
        /// <paramref name="sync"/> が <see cref="SynchronizationContext.Current"/> と違うときだけ Post。
        /// 同じなら直接アクション呼び出し。
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <param name="sync"></param>
        /// <param name="action"></param>
        /// <param name="state"></param>
        public static void PostIfNotCurrent<TState>(this SynchronizationContext sync, Action<TState> action, TState state)
        {
            if (sync != null && sync != SynchronizationContext.Current)
                sync.Post(x => action((TState)x), state);
            else
                action(state);
        }

        /// <summary>
        /// null チェック付き
        /// <paramref name="sync"/> が <see cref="SynchronizationContext.Current"/> と違うときだけ Post。
        /// 同じなら直接アクション呼び出し。
        /// </summary>
        /// <param name="sync"></param>
        /// <param name="func"></param>
        public static Task<TResult> PostIfNotCurrentAsync<TResult>(this SynchronizationContext sync, Func<TResult> func)
        {
            if (sync != null && sync != SynchronizationContext.Current)
            {
                var tcs = new TaskCompletionSource<TResult>();
                sync.Post(x =>
                {
                    try
                    {
                        var r = func();
                        tcs.TrySetResult(r);
                    }
                    catch (Exception ex)
                    {
                        tcs.TrySetException(ex);
                    }
                }, null);

                return tcs.Task;
            }
            else
            {
                return Task.FromResult(func());
            }
        }

        /// <summary>
        /// null チェック付き
        /// <paramref name="sync"/> が <see cref="SynchronizationContext.Current"/> と違うときだけ Post。
        /// 同じなら直接アクション呼び出し。
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="sync"></param>
        /// <param name="func"></param>
        /// <param name="state"></param>
        public static Task<TResult> PostIfNotCurrentAsync<TState, TResult>(this SynchronizationContext sync, Func<TState, TResult> func, TState state)
        {
            if (sync != null && sync != SynchronizationContext.Current)
            {
                var tcs = new TaskCompletionSource<TResult>();
                sync.Post(x =>
                {
                    try
                    {
                        var r = func(state);
                        tcs.TrySetResult(r);
                    }
                    catch (Exception ex)
                    {
                        tcs.TrySetException(ex);
                    }
                }, null);

                return tcs.Task;
            }
            else
            {
                return Task.FromResult(func(state));
            }
        }

        /// <summary>
        /// null チェック付き
        /// <paramref name="sync"/> が <see cref="SynchronizationContext.Current"/> と違うときだけ Post。
        /// 同じなら直接アクション呼び出し。
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <param name="sync"></param>
        /// <param name="func"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public static Task PostIfNotCurrentAsync<TState>(this SynchronizationContext sync, Action<TState> func, TState state)
            => PostIfNotCurrentAsync(sync, new Func<TState, object>(x => { func(x); return null; }), state);

        /// <summary>
        /// null チェック付き
        /// <paramref name="sync"/> が <see cref="SynchronizationContext.Current"/> と違うときだけ Post。
        /// 同じなら直接アクション呼び出し。
        /// </summary>
        /// <param name="sync"></param>
        /// <param name="actionAsync"></param>
        /// <returns></returns>
        public static Task PostIfNotCurrentAsync(this SynchronizationContext sync, Func<Task> actionAsync)
        {
            if (sync != null && sync != SynchronizationContext.Current)
                return PostAsync(sync, actionAsync);
            else
                return actionAsync();
        }

        /// <summary>
        /// null チェック付き
        /// <paramref name="sync"/> が <see cref="SynchronizationContext.Current"/> と違うときだけ Post。
        /// 同じなら直接アクション呼び出し。
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <param name="sync"></param>
        /// <param name="actionAsync"></param>
        /// <param name="asyncState"></param>
        /// <returns></returns>
        public static Task PostIfNotCurrentAsync<TState>(this SynchronizationContext sync, Func<TState, Task> actionAsync, TState asyncState)
        {
            if (sync != null && sync != SynchronizationContext.Current)
                return PostAsync(sync, actionAsync, asyncState);
            else
                return actionAsync(asyncState);
        }

        /// <summary>
        /// null チェック付き
        /// <paramref name="sync"/> が <see cref="SynchronizationContext.Current"/> と違うときだけ Post。
        /// 同じなら直接アクション呼び出し。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sync"></param>
        /// <param name="actionAsync"></param>
        /// <returns></returns>
        public static Task<T> PostIfNotCurrentAsync<T>(this SynchronizationContext sync, Func<Task<T>> actionAsync)
        {
            if (sync != null && sync != SynchronizationContext.Current)
                return PostAsync(sync, actionAsync);
            else
                return actionAsync();
        }

        /// <summary>
        /// null チェック付き
        /// <paramref name="sync"/> が <see cref="SynchronizationContext.Current"/> と違うときだけ Post。
        /// 同じなら直接アクション呼び出し。
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="sync"></param>
        /// <param name="actionAsync"></param>
        /// <param name="asyncState"></param>
        /// <returns></returns>
        public static Task<TResult> PostIfNotCurrentAsync<TState, TResult>(this SynchronizationContext sync, Func<TState, Task<TResult>> actionAsync, TState asyncState)
        {
            if (sync != null && sync != SynchronizationContext.Current)
                return PostAsync(sync, actionAsync, asyncState);
            else
                return actionAsync(asyncState);
        }

        /// <summary>
        /// <see cref="SynchronizationContext.Post(SendOrPostCallback, object)"/> 内でタスクを起動。
        /// </summary>
        /// <param name="sync"></param>
        /// <param name="actionAsync"></param>
        /// <returns></returns>
        public static Task PostAsync(this SynchronizationContext sync, Func<Task> actionAsync)
        {
            var tcs = new TaskCompletionSource<object>();

            sync.Post(async x =>
            {
                try
                {
                    await actionAsync();
                    tcs.TrySetResult(null);
                }
                catch(Exception ex)
                {
                    tcs.TrySetException(ex);
                }

            }, null);

            return tcs.Task;
        }

        /// <summary>
        /// <see cref="SynchronizationContext.Post(SendOrPostCallback, object)"/> 内でタスクを起動。
        /// </summary>
        /// <param name="sync"></param>
        /// <param name="actionAsync"></param>
        /// <param name="asyncState"></param>
        /// <returns></returns>
        public static Task PostAsync<TState>(this SynchronizationContext sync, Func<TState, Task> actionAsync, TState asyncState)
        {
            var tcs = new TaskCompletionSource<object>();

            sync.Post(async x =>
            {
                try
                {
                    await actionAsync((TState)x);
                    tcs.TrySetResult(null);
                }
                catch (Exception ex)
                {
                    tcs.TrySetException(ex);
                }

            }, asyncState);

            return tcs.Task;
        }

        /// <summary>
        /// <see cref="SynchronizationContext.Post(SendOrPostCallback, object)"/> 内でタスクを起動。
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="sync"></param>
        /// <param name="actionAsync"></param>
        /// <returns></returns>
        public static Task<TResult> PostAsync<TResult>(this SynchronizationContext sync, Func<Task<TResult>> actionAsync)
        {
            var tcs = new TaskCompletionSource<TResult>();

            sync.Post(async x =>
            {
                try
                {
                    var r = await actionAsync();
                    tcs.TrySetResult(r);
                }
                catch (Exception ex)
                {
                    tcs.TrySetException(ex);
                }

            }, null);

            return tcs.Task;
        }

        /// <summary>
        /// <see cref="SynchronizationContext.Post(SendOrPostCallback, object)"/> 内でタスクを起動。
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="sync"></param>
        /// <param name="actionAsync"></param>
        /// <param name="asyncState"></param>
        /// <returns></returns>
        public static Task<TResult> PostAsync<TState, TResult>(this SynchronizationContext sync, Func<TState, Task<TResult>> actionAsync, TState asyncState)
        {
            var tcs = new TaskCompletionSource<TResult>();

            sync.Post(async x =>
            {
                try
                {
                    var r = await actionAsync((TState)x);
                    tcs.TrySetResult(r);
                }
                catch (Exception ex)
                {
                    tcs.TrySetException(ex);
                }

            }, asyncState);

            return tcs.Task;
        }
    }
}
