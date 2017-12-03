namespace ByRef.StackOnly
{
    using System;
    using System.Collections;
    using System.Threading.Tasks;

    class Program
    {
#if InvalidCode
        void M(ref int x)
        {
            // クロージャに使えない
            Action<int> a = i => x = i;
            void f(int i) => x = i;
        }

        // イテレーターの引数に使えない
        IEnumerable Iterator(ref int x)
        {
            yield break;
        }

        // 非同期メソッドの引数に使えない
        async Task Async(ref int x)
        {
            await Task.Delay(1);
        }
#endif
    }
}
