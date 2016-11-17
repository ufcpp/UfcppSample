using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;

namespace ContextFreeTasks
{
    [StructLayout(LayoutKind.Auto)]
    public struct AsyncContextFreeTaskMethodBuilder
    {
        private AsyncTaskMethodBuilder _methodBuilder;
        public static AsyncContextFreeTaskMethodBuilder Create() =>
            new AsyncContextFreeTaskMethodBuilder() { _methodBuilder = AsyncTaskMethodBuilder.Create() };
        public void Start<TStateMachine>(ref TStateMachine stateMachine) where TStateMachine : IAsyncStateMachine => _methodBuilder.Start(ref stateMachine);
        public void SetStateMachine(IAsyncStateMachine stateMachine) => _methodBuilder.SetStateMachine(stateMachine);
        public void SetResult() => _methodBuilder.SetResult();
        public void SetException(Exception exception) => _methodBuilder.SetException(exception);
        public ContextFreeTask Task => new ContextFreeTask(_methodBuilder.Task);
        public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : INotifyCompletion
            where TStateMachine : IAsyncStateMachine
        {
            var t = UnsafeHelper.ExtractTask(awaiter);
            if (t != null)
            {
                var cawaiter = t.ConfigureAwait(false).GetAwaiter();
                _methodBuilder.AwaitOnCompleted(ref cawaiter, ref stateMachine);
            }
            else
            {
                _methodBuilder.AwaitOnCompleted(ref awaiter, ref stateMachine);
            }
        }
        [SecuritySafeCritical]
        public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : ICriticalNotifyCompletion
            where TStateMachine : IAsyncStateMachine
        {
            var t = UnsafeHelper.ExtractTask(awaiter);
            if (t != null)
            {
                var cawaiter = t.ConfigureAwait(false).GetAwaiter();
                _methodBuilder.AwaitUnsafeOnCompleted(ref cawaiter, ref stateMachine);
            }
            else
            {
                _methodBuilder.AwaitUnsafeOnCompleted(ref awaiter, ref stateMachine);
            }
        }
    }
}
