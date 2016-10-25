using System;
using System.Runtime.CompilerServices;

[AsyncMethodBuilder(typeof(AsyncValueTaskMethodBuilder<>))]
struct TaskLike<TResult>
{
}

struct AsyncValueTaskMethodBuilder<TResult>
{
    public static AsyncValueTaskMethodBuilder<TResult> Create() => default(AsyncValueTaskMethodBuilder<TResult>);
    public void Start<TStateMachine>(ref TStateMachine stateMachine) where TStateMachine : IAsyncStateMachine { }
    public void SetStateMachine(IAsyncStateMachine stateMachine) { }
    public void SetResult(TResult result) { }
    public void SetException(Exception exception) { }
    public TaskLike<TResult> Task { get; }
    public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
        where TAwaiter : INotifyCompletion
        where TStateMachine : IAsyncStateMachine
    { }
    public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
        where TAwaiter : ICriticalNotifyCompletion
        where TStateMachine : IAsyncStateMachine
    { }
}

#if false
namespace System.Runtime.CompilerServices
{
    sealed class AsyncMethodBuilderAttribute : Attribute
    {
        public AsyncMethodBuilderAttribute(Type builderType)
        {
            BuilderType = builderType;
        }

        public Type BuilderType { get; }
    }
}
#endif
