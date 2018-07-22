using System;
using System.Diagnostics;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;

namespace CachedAsync
{
    internal class AsyncOperation
    {
        /// <summary>Sentinel object used in a field to indicate the operation is available for use.</summary>
        protected static readonly Action<object> s_availableSentinel = new Action<object>(s => Debug.Fail($"{nameof(AsyncOperation)}.{nameof(s_availableSentinel)} invoked with {s}."));
        /// <summary>Sentinel object used in a field to indicate the operation has completed.</summary>
        protected static readonly Action<object> s_completedSentinel = new Action<object>(s => Debug.Fail($"{nameof(AsyncOperation)}.{nameof(s_completedSentinel)} invoked with {s}"));

        protected static void ThrowIncompleteOperationException() =>
            throw new InvalidOperationException("the operation's result was accessed before the operation completed.");

        protected static void ThrowMultipleContinuations() =>
            throw new InvalidOperationException("multiple continuations can't be set for the same operation.");

        protected static void ThrowIncorrectCurrentIdException() =>
            throw new InvalidOperationException("the operation was used after it was supposed to be used.");
    }

    internal class AsyncOperation<TResult> : AsyncOperation, IValueTaskSource, IValueTaskSource<TResult>
    {
        /// <summary>Whether continuations should be forced to run asynchronously.</summary>
        private readonly bool _runContinuationsAsynchronously;

        /// <summary>The result of the operation.</summary>
        private TResult _result;
        /// <summary>Any error that occurred during the operation.</summary>
        private ExceptionDispatchInfo _error;
        /// <summary>The continuation callback.</summary>
        /// <remarks>
        /// This may be the completion sentinel if the operation has already completed.
        /// This may be the available sentinel if the operation is being pooled and is available for use.
        /// This may be null if the operation is pending.
        /// This may be another callback if the operation has had a callback hooked up with OnCompleted.
        /// </remarks>
        private Action<object> _continuation;
        /// <summary>State object to be passed to <see cref="_continuation"/>.</summary>
        private object _continuationState;
        /// <summary>Scheduling context (a <see cref="SynchronizationContext"/> or <see cref="TaskScheduler"/>) to which to queue the continuation. May be null.</summary>
        private object _schedulingContext;
        /// <summary>The token value associated with the current operation.</summary>
        /// <remarks>
        /// IValueTaskSource operations on this instance are only valid if the provided token matches this value,
        /// which is incremented once GetResult is called to avoid multiple awaits on the same instance.
        /// </remarks>
        private short _currentId;

        /// <summary>Initializes the interactor.</summary>
        /// <param name="runContinuationsAsynchronously">true if continuations should be forced to run asynchronously; otherwise, false.</param>
        /// <param name="pooled">Whether this instance is pooled and reused.</param>
        public AsyncOperation(bool runContinuationsAsynchronously)
        {
            _continuation = s_availableSentinel;
            _runContinuationsAsynchronously = runContinuationsAsynchronously;
        }

        /// <summary>Gets or sets the next operation in the linked list of operations.</summary>
        public AsyncOperation<TResult> Next { get; set; }
        /// <summary>Gets a <see cref="ValueTask"/> backed by this instance and its current token.</summary>
        public ValueTask ValueTask => new ValueTask(this, _currentId);
        /// <summary>Gets a <see cref="ValueTask{TResult}"/> backed by this instance and its current token.</summary>
        public ValueTask<TResult> ValueTaskOfT => new ValueTask<TResult>(this, _currentId);

        /// <summary>Gets the current status of the operation.</summary>
        /// <param name="token">The token that must match <see cref="_currentId"/>.</param>
        public ValueTaskSourceStatus GetStatus(short token)
        {
            if (_currentId == token)
            {
                return
                    !IsCompleted ? ValueTaskSourceStatus.Pending :
                    _error == null ? ValueTaskSourceStatus.Succeeded :
                    _error.SourceException is OperationCanceledException ? ValueTaskSourceStatus.Canceled :
                    ValueTaskSourceStatus.Faulted;
            }

            ThrowIncorrectCurrentIdException();
            return default; // just to satisfy compiler
        }

        /// <summary>Gets whether the operation has completed.</summary>
        /// <remarks>
        /// The operation is considered completed if both a) it's in the completed state,
        /// AND b) it has a non-null continuation.  We need to consider both because they're
        /// not set atomically.  If we only considered the state, then if we set the state to
        /// completed and then set the continuation, it's possible for an awaiter to check
        /// IsCompleted, see true, call GetResult, and return the object to the pool, and only
        /// then do we try to store the continuation into an object we no longer own.  If we
        /// only considered the state, then if we set the continuation and then set the state,
        /// a racing awaiter could see the continuation set before the state has transitioned
        /// to completed and could end up calling GetResult in an incomplete state.  And if we
        /// only considered the continuation, then we have issues if OnCompleted is used before
        /// the operation completes, as the continuation will be 
        /// </remarks>
        internal bool IsCompleted => ReferenceEquals(_continuation, s_completedSentinel);

        /// <summary>Gets the result of the operation.</summary>
        /// <param name="token">The token that must match <see cref="_currentId"/>.</param>
        public TResult GetResult(short token)
        {
            if (_currentId != token)
            {
                ThrowIncorrectCurrentIdException();
            }

            if (!IsCompleted)
            {
                ThrowIncompleteOperationException();
            }

            ExceptionDispatchInfo error = _error;
            TResult result = _result;
            _currentId++;

            Volatile.Write(ref _continuation, s_availableSentinel); // only after fetching all needed data

            error?.Throw();
            return result;
        }

        /// <summary>Gets the result of the operation.</summary>
        /// <param name="token">The token that must match <see cref="_currentId"/>.</param>
        void IValueTaskSource.GetResult(short token)
        {
            if (_currentId != token)
            {
                ThrowIncorrectCurrentIdException();
            }

            if (!IsCompleted)
            {
                ThrowIncompleteOperationException();
            }

            ExceptionDispatchInfo error = _error;
            _currentId++;

            Volatile.Write(ref _continuation, s_availableSentinel); // only after fetching all needed data

            error?.Throw();
        }

        /// <summary>Attempts to take ownership of the pooled instance.</summary>
        /// <returns>true if the instance is now owned by the caller, in which case its state has been reset; otherwise, false.</returns>
        public bool TryOwnAndReset()
        {
            if (ReferenceEquals(Interlocked.CompareExchange(ref _continuation, null, s_availableSentinel), s_availableSentinel))
            {
                _continuationState = null;
                _result = default;
                _error = null;
                _schedulingContext = null;
                return true;
            }

            return false;
        }

        /// <summary>Hooks up a continuation callback for when the operation has completed.</summary>
        /// <param name="continuation">The callback.</param>
        /// <param name="state">The state to pass to the callback.</param>
        /// <param name="token">The current token that must match <see cref="_currentId"/>.</param>
        /// <param name="flags">Flags that influence the behavior of the callback.</param>
        public void OnCompleted(Action<object> continuation, object state, short token, ValueTaskSourceOnCompletedFlags flags)
        {
            if (_currentId != token)
            {
                ThrowIncorrectCurrentIdException();
            }

            // We need to store the state before the CompareExchange, so that if it completes immediately
            // after the CompareExchange, it'll find the state already stored.  If someone misuses this
            // and schedules multiple continuations erroneously, we could end up using the wrong state.
            // Make a best-effort attempt to catch such misuse.
            if (_continuationState != null)
            {
                ThrowMultipleContinuations();
            }
            _continuationState = state;

            // Capture the scheduling context if necessary.
            Debug.Assert(_schedulingContext == null);
            SynchronizationContext sc = null;
            TaskScheduler ts = null;
            if ((flags & ValueTaskSourceOnCompletedFlags.UseSchedulingContext) != 0)
            {
                sc = SynchronizationContext.Current;
                if (sc != null && sc.GetType() != typeof(SynchronizationContext))
                {
                    _schedulingContext = sc;
                }
                else
                {
                    ts = TaskScheduler.Current;
                    if (ts != TaskScheduler.Default)
                    {
                        _schedulingContext = ts;
                    }
                }
            }

            // Try to set the provided continuation into _continuation.  If this succeeds, that means the operation
            // has not yet completed, and the completer will be responsible for invoking the callback.  If this fails,
            // that means the operation has already completed, and we must invoke the callback, but because we're still
            // inside the awaiter's OnCompleted method and we want to avoid possible stack dives, we must invoke
            // the continuation asynchronously rather than synchronously.
            Action<object> prevContinuation = Interlocked.CompareExchange(ref _continuation, continuation, null);
            if (prevContinuation != null)
            {
                // If the set failed because there's already a delegate in _continuation, but that delegate is
                // something other than s_completedSentinel, something went wrong, which should only happen if
                // the instance was erroneously used, likely to hook up multiple continuations.
                Debug.Assert(IsCompleted, $"Expected IsCompleted");
                if (!ReferenceEquals(prevContinuation, s_completedSentinel))
                {
                    Debug.Assert(prevContinuation != s_availableSentinel, "Continuation was the available sentinel.");
                    ThrowMultipleContinuations();
                }

                // Queue the continuation.
                if (sc != null)
                {
                    sc.Post(s =>
                    {
                        var t = (Tuple<Action<object>, object>)s;
                        t.Item1(t.Item2);
                    }, Tuple.Create(continuation, state));
                }
                else
                {
                    Task.Factory.StartNew(continuation, state, CancellationToken.None, TaskCreationOptions.DenyChildAttach, ts ?? TaskScheduler.Default);
                }
            }
        }

        /// <summary>Completes the operation with a success state and the specified result.</summary>
        /// <param name="item">The result value.</param>
        /// <returns>true if the operation could be successfully transitioned to a completed state; false if it was already completed.</returns>
        public bool TrySetResult(TResult item)
        {
            _result = item;
            SignalCompletion();
            return true;
        }

        /// <summary>Completes the operation with a failed state and the specified error.</summary>
        /// <param name="exception">The error.</param>
        /// <returns>true if the operation could be successfully transitioned to a completed state; false if it was already completed.</returns>
        public bool TrySetException(Exception exception)
        {
            _error = ExceptionDispatchInfo.Capture(exception);
            SignalCompletion();
            return true;
        }

        /// <summary>Completes the operation with a failed state and a cancellation error.</summary>
        /// <param name="cancellationToken">The cancellation token that caused the cancellation.</param>
        /// <returns>true if the operation could be successfully transitioned to a completed state; false if it was already completed.</returns>
        public bool TrySetCanceled(CancellationToken cancellationToken = default)
        {
            _error = ExceptionDispatchInfo.Capture(new OperationCanceledException(cancellationToken));
            SignalCompletion();
            return true;
        }

        /// <summary>Signals to a registered continuation that the operation has now completed.</summary>
        private void SignalCompletion()
        {
            if (_continuation != null || Interlocked.CompareExchange(ref _continuation, s_completedSentinel, null) != null)
            {
                SignalCompletionCore();
            }
        }

        /// <summary>Invokes the registered continuation; separated out of SignalCompletion for convenience so that it may be invoked on multiple code paths.</summary>
        private void SignalCompletionCore()
        {
            Debug.Assert(_continuation != s_completedSentinel, $"The continuation was the completion sentinel.");
            Debug.Assert(_continuation != s_availableSentinel, $"The continuation was the available sentinel.");

            if (_schedulingContext == null)
            {
                // There's no captured scheduling context.  If we're forced to run continuations asynchronously, queue it.
                // Otherwise fall through to invoke it synchronously.
                if (_runContinuationsAsynchronously)
                {
                    Task.Factory.StartNew(s => ((AsyncOperation<TResult>)s).SetCompletionAndInvokeContinuation(), this,
                        CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);
                    return;
                }
            }
            else if (_schedulingContext is SynchronizationContext sc)
            {
                // There's a captured synchronization context.  If we're forced to run continuations asynchronously,
                // or if there's a current synchronization context that's not the one we're targeting, queue it.
                // Otherwise fall through to invoke it synchronously.
                if (_runContinuationsAsynchronously || sc != SynchronizationContext.Current)
                {
                    sc.Post(s => ((AsyncOperation<TResult>)s).SetCompletionAndInvokeContinuation(), this);
                    return;
                }
            }
            else
            {
                // There's a captured TaskScheduler.  If we're forced to run continuations asynchronously,
                // or if there's a current scheduler that's not the one we're targeting, queue it.
                // Otherwise fall through to invoke it synchronously.
                TaskScheduler ts = (TaskScheduler)_schedulingContext;
                Debug.Assert(ts != null, "Expected a TaskScheduler");
                if (_runContinuationsAsynchronously || ts != TaskScheduler.Current)
                {
                    Task.Factory.StartNew(s => ((AsyncOperation<TResult>)s).SetCompletionAndInvokeContinuation(), this,
                        CancellationToken.None, TaskCreationOptions.DenyChildAttach, ts);
                    return;
                }
            }

            // Invoke the continuation synchronously.
            SetCompletionAndInvokeContinuation();
        }

        private void SetCompletionAndInvokeContinuation()
        {
            Action<object> c = _continuation;
            _continuation = s_completedSentinel;
            c(_continuationState);
        }
    }
}

