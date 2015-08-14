using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.System;

namespace NativeInterop
{
    class WinRtSample
    {
        public static void Main()
        {
            MainAsync().Wait();
        }

        static async Task MainAsync()
        {
            var allUsers = await User.FindAllAsync();

            foreach (var user in allUsers)
            {
                Console.WriteLine(user.NonRoamableId);
            }
        }
    }

    static class WinRtExtensions
    {
        public static TaskAwaiter<T> GetAwaiter<T>(this IAsyncOperation<T> t) => t.AsTask().GetAwaiter();

        public static Task<T> AsTask<T>(this IAsyncOperation<T> t)
        {
            var tcs = new TaskCompletionSource<T>();
            t.Completed += (info, state) =>
            {
                try
                {
                    tcs.TrySetResult(info.GetResults());
                }
                catch(Exception ex)
                {
                    tcs.TrySetException(ex);
                }
            };
            return tcs.Task;
        }
    }
}
