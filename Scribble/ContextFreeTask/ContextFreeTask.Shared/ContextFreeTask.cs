using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace ContextFreeTasks
{
    /// <summary>
    /// - 単なる <see cref="Task"/> クラスのラッパー
    /// - 非同期メソッドの戻り値に使える。その場合、非同期メソッド内の await はすべて ConfigureAwait(false) 状態になる
    /// - await できる。その await は ConfigureAwait(false) 状態になる
    /// </summary>
    [AsyncMethodBuilder(typeof(AsyncContextFreeTaskMethodBuilder))]
    public struct ContextFreeTask
    {
        public Task Task { get; }
        public ContextFreeTask(Task t) => Task = t;
        public ContextFreeTaskAwaiter GetAwaiter() => new ContextFreeTaskAwaiter(Task);
    }
}
