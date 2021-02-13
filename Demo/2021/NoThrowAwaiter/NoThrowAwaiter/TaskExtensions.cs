using System.Threading.Tasks;

namespace NoThrowAwaiter
{
    public static class TaskExtensions
    {
        public static NoThrowOnCancelAwaiter NoThrowOnCancel(this Task t, bool continueOnCapturedContext = true) => new(t, continueOnCapturedContext);
    }
}
