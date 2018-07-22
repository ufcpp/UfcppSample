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
        internal static readonly Action<object> s_availableSentinel = new Action<object>(s => Debug.Fail($"{nameof(AsyncOperation)}.{nameof(s_availableSentinel)} invoked with {s}."));
        /// <summary>Sentinel object used in a field to indicate the operation has completed.</summary>
        internal static readonly Action<object> s_completedSentinel = new Action<object>(s => Debug.Fail($"{nameof(AsyncOperation)}.{nameof(s_completedSentinel)} invoked with {s}"));

        internal static void ThrowIncompleteOperationException() =>
            throw new InvalidOperationException("the operation's result was accessed before the operation completed.");

        internal static void ThrowMultipleContinuations() =>
            throw new InvalidOperationException("multiple continuations can't be set for the same operation.");

        internal static void ThrowIncorrectCurrentIdException() =>
            throw new InvalidOperationException("the operation was used after it was supposed to be used.");
    }

    public class AsyncOperation<TResult> : IValueTaskSource<TResult>
    {
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
        private Action<object> _continuation = AsyncOperation.s_availableSentinel;
        /// <summary>State object to be passed to <see cref="_continuation"/>.</summary>
        private object _continuationState;
        /// <summary>The token value associated with the current operation.</summary>
        /// <remarks>
        /// IValueTaskSource operations on this instance are only valid if the provided token matches this value,
        /// which is incremented once GetResult is called to avoid multiple awaits on the same instance.
        /// </remarks>
        private short _currentId;

        /// <summary>Gets a <see cref="ValueTask{TResult}"/> backed by this instance and its current token.</summary>
        public ValueTask<TResult> Task => new ValueTask<TResult>(this, _currentId);

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

            AsyncOperation.ThrowIncorrectCurrentIdException();
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
        internal bool IsCompleted => ReferenceEquals(_continuation, AsyncOperation.s_completedSentinel);

        /// <summary>Gets the result of the operation.</summary>
        /// <param name="token">The token that must match <see cref="_currentId"/>.</param>
        public TResult GetResult(short token)
        {
            if (_currentId != token)
            {
                AsyncOperation.ThrowIncorrectCurrentIdException();
            }

            if (!IsCompleted)
            {
                AsyncOperation.ThrowIncompleteOperationException();
            }

            ExceptionDispatchInfo error = _error;
            TResult result = _result;
            _currentId++;

            Volatile.Write(ref _continuation, AsyncOperation.s_availableSentinel); // only after fetching all needed data

            error?.Throw();
            return result;
        }

        /// <summary>Attempts to take ownership of the pooled instance.</summary>
        /// <returns>true if the instance is now owned by the caller, in which case its state has been reset; otherwise, false.</returns>
        public bool TryOwnAndReset()
        {
            if (ReferenceEquals(Interlocked.CompareExchange(ref _continuation, null, AsyncOperation.s_availableSentinel), AsyncOperation.s_availableSentinel))
            {
                _continuationState = null;
                _result = default;
                _error = null;
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
                AsyncOperation.ThrowIncorrectCurrentIdException();
            }

            // We need to store the state before the CompareExchange, so that if it completes immediately
            // after the CompareExchange, it'll find the state already stored.  If someone misuses this
            // and schedules multiple continuations erroneously, we could end up using the wrong state.
            // Make a best-effort attempt to catch such misuse.
            if (_continuationState != null)
            {
                AsyncOperation.ThrowMultipleContinuations();
            }
            _continuationState = state;

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
                if (!ReferenceEquals(prevContinuation, AsyncOperation.s_completedSentinel))
                {
                    Debug.Assert(prevContinuation != AsyncOperation.s_availableSentinel, "Continuation was the available sentinel.");
                    AsyncOperation.ThrowMultipleContinuations();
                }

                //// Queue the continuation.
                //Task.Factory.StartNew(continuation, state, CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);
                continuation(state);
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
            if (_continuation != null || Interlocked.CompareExchange(ref _continuation, AsyncOperation.s_completedSentinel, null) != null)
            {
                Debug.Assert(_continuation != AsyncOperation.s_completedSentinel, $"The continuation was the completion sentinel.");
                Debug.Assert(_continuation != AsyncOperation.s_availableSentinel, $"The continuation was the available sentinel.");

                Action<object> c = _continuation;
                _continuation = AsyncOperation.s_completedSentinel;
                c(_continuationState);
            }
        }
    }
}

