using System;
using System.Threading;

namespace ThreadPost
{
    internal readonly struct SynchronizationContextPostItem
    {
        private readonly SendOrPostCallback _callback;
        private readonly object _state;

        public SynchronizationContextPostItem(SendOrPostCallback callback, object state)
        {
            _callback = callback;
            _state = state;
        }

        public void Invoke(UnhandledExceptionEventHandler exceptionHandler)
        {
            try
            {
                _callback(_state);
            }
            catch (Exception ex)
            {
                exceptionHandler?.Invoke(this, new UnhandledExceptionEventArgs(ex, false));
            }
        }
    }
}
