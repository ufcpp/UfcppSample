using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace ContextFreeTasks
{
    /// <summary>
    /// - 単なる <see cref="Task{TResult}"/> クラスのラッパー
    /// <see cref="ContextFreeTask"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [AsyncMethodBuilder(typeof(AsyncContextFreeTaskMethodBuilder<>))]
    public struct ContextFreeTask<T>
    {
        public Task<T> Task { get; }
        public ContextFreeTask(Task<T> t) => Task = t;
        public ContextFreeTaskAwaiter<T> GetAwaiter() => new ContextFreeTaskAwaiter<T>(Task);
    }
}
