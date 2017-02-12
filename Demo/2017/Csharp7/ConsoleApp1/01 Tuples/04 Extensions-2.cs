using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace ConsoleApp1._01_Tuples
{
    static class Extensions2
    {
        // 実用の例。タプルを使った拡張メソッドを2つほど紹介
        // 2つ目
        public static async Task Run()
        {
            // 複数の Task をタプルを使って await
            await (Task.Delay(1), Task.Delay(1));
        }

        public static TaskAwaiter GetAwaiter(this (Task t1, Task t2) t) => Task.WhenAll(t.t1, t.t2).GetAwaiter();
    }
}