using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AsyncInternal
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Write(Synchronous.GetContents());

            var tcs = new TaskCompletionSource<IEnumerable<string>>();
            CallbackAsync.GetContents(tcs.SetResult);
            Write(await tcs.Task);

            Write(await AwaitOperator.GetContents());

            Write(await AwaitCodeGeneration.GetContents());
        }

        static void Write(IEnumerable<string> contents)
        {
            foreach (var x in contents)
            {
                Console.WriteLine(x);
            }
        }
    }
}
