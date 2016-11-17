using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace ContextFreeTasks
{
    public struct ContextFreeTaskAwaiter<T> : ICriticalNotifyCompletion
    {
        private readonly Task<T> _value;
        internal ContextFreeTaskAwaiter(Task<T> value) { _value = value; }
        public bool IsCompleted => _value.IsCompleted;
        public T GetResult() => _value.GetAwaiter().GetResult();
        public void OnCompleted(Action continuation) => _value.ConfigureAwait(false).GetAwaiter().OnCompleted(continuation);
        public void UnsafeOnCompleted(Action continuation) => _value.ConfigureAwait(false).GetAwaiter().UnsafeOnCompleted(continuation);
    }
}
