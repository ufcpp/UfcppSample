using System;
using System.Threading.Tasks;

namespace NoThrowAwaiter
{
    public struct ResultOrCancel
    {
        private ResultOrCancel(bool ranToCompetion) => IsCompletedSuccessfully = ranToCompetion;

        public bool IsCompletedSuccessfully { get; }
        public bool IsCanceled => !IsCompletedSuccessfully;

        internal static ResultOrCancel FromTask(Task t)
        {
            if (t.IsCanceled) return default;
            if (t.Exception?.InnerException is OperationCanceledException) return default;
            t.GetAwaiter().GetResult();
            return new(true);
        }
    }
}
