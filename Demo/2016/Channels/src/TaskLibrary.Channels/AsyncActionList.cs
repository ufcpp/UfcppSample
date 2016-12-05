using SystemAsync;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TaskLibrary.Channels
{
    /// <summary>
    /// <see cref="System.MulticastDelegate"/>みたいなもの。
    /// </summary>
    /// <typeparam name="T">引数の型。</typeparam>
    /// <remarks>
    /// 戻り値のあるデリゲートの += (<see cref="System.Delegate.Combine(System.Delegate, System.Delegate)"/>)は、最初の1個の戻り値しか返さない。
    /// 非同期メソッドをawaitするためには戻り値の<see cref="Task"/>が必要なわけで、<see cref="System.MulticastDelegate"/>は使えない。
    ///
    /// 引数の個数違いのやつを作るのは大変すぎるし、<see cref="ISender{TMessage}"/>用と割り切ってるので、1引数版しか作らない。
    ///
    /// <see cref="Add(AsyncAction{T})"/>とかの中で<see cref="Interlocked.CompareExchange{T}(ref T, T, T)"/>を使ってる。
    /// 古いUnityだとこのメソッドが呼べなかったんだけども。今なら使えると思う。たぶん。要確認。
    /// </remarks>
    internal struct AsyncActionList<T>
    {
        public AsyncAction<T>[] _actions;

        public void Add(AsyncAction<T> action)
        {
            AsyncAction<T>[] x2;
            var x1 = _actions;
            do
            {
                x2 = x1;
                var x3 = x2 == null ? new[] { action } : x2.Append(action).ToArray();
                x1 = Interlocked.CompareExchange(ref _actions, x3, x2);
            }
            while (x1 != x2);
        }

        public void Remove(AsyncAction<T> action)
        {
            AsyncAction<T>[] x2;
            var x1 = _actions;
            do
            {
                x2 = x1;
                var x3 = x2.Where(x => x != action).ToArray();
                x1 = Interlocked.CompareExchange(ref _actions, x3, x2);
            }
            while (x1 != x2);
        }

        public async Task InvokeSequentialAsync(T args, CancellationToken ct)
        {
            var list = _actions;
            if (list == null || list.Length == 0) return;
            foreach (var x in list)
            {
                await x(args, ct);
            }
        }

        public Task InvokeParallelAsync(T args, CancellationToken ct)
        {
            var list = _actions;
            if (list == null || list.Length == 0) return Task.CompletedTask;
            return Task.WhenAll(list.Select(x => x(args, ct)));
        }
    }

    internal static class AsyncActionListExtensions
    {
        internal static Task InvokeAsync<T>(this AsyncActionList<T> list, InvocationMode mode, T args, CancellationToken ct)
            => mode == InvocationMode.Sequential
            ? list.InvokeSequentialAsync(args, ct)
            : list.InvokeParallelAsync(args, ct);
    }
}
