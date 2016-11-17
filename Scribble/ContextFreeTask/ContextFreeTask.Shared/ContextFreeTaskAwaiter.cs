using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace ContextFreeTasks
{
    public struct ContextFreeTaskAwaiter : ICriticalNotifyCompletion
    {
        private readonly Task _value;
        internal ContextFreeTaskAwaiter(Task value) { _value = value; }
        public bool IsCompleted => _value.IsCompleted;
        public void GetResult() => _value.GetAwaiter().GetResult();
        public void OnCompleted(Action continuation) => _value.ConfigureAwait(false).GetAwaiter().OnCompleted(continuation);
        public void UnsafeOnCompleted(Action continuation) => _value.ConfigureAwait(false).GetAwaiter().UnsafeOnCompleted(continuation);
    }
}
