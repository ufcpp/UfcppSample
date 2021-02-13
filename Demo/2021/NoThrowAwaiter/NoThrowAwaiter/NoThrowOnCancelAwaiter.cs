using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace NoThrowAwaiter
{
    public struct NoThrowOnCancelAwaiter : ICriticalNotifyCompletion
    {
        private readonly Task _t;
        private readonly bool _continueOnCapturedContext;
        internal NoThrowOnCancelAwaiter(Task task, bool continueOnCapturedContext) { _t = task; _continueOnCapturedContext = continueOnCapturedContext; }
        public bool IsCompleted { get { return _t.IsCompleted; } }
        public ResultOrCancel GetResult() => ResultOrCancel.FromTask(_t);
        public void OnCompleted(Action continuation) => _t.ConfigureAwait(_continueOnCapturedContext).GetAwaiter().OnCompleted(continuation);
        public void UnsafeOnCompleted(Action continuation) => _t.ConfigureAwait(_continueOnCapturedContext).GetAwaiter().UnsafeOnCompleted(continuation);
        public NoThrowOnCancelAwaiter GetAwaiter() => this;
    }
}
