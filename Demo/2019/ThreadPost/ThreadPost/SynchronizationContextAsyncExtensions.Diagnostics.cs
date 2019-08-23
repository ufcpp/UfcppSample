using System.Collections.Concurrent;

namespace ThreadPost
{
    public static partial class SynchronizationContextAsyncExtensions
    {
        public static int GetFuncDiagCount<TState, TResult>() => GetDiagCount<StateFuncValueTaskSource<TState, TResult>>();
        public static int GetFuncDiagCount<TResult>() => GetDiagCount<FuncValueTaskSource<TResult>>();
        public static int GetActionDiagCount<TState>() => GetDiagCount<StateActionValueTaskSource<TState>>();
        public static int GetActionDiagCount() => GetDiagCount<ActionValueTaskSource>();

        public static int GetDiagCount<T>()
        {
            var p = typeof(T).GetField("_pool", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
            var q = typeof(ObjectPool<T>).GetField("_queue", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            var c = typeof(ConcurrentQueue<T>).GetProperty("Count", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            return (int)c.GetValue(q.GetValue(p.GetValue(null)));
        }
    }
}
