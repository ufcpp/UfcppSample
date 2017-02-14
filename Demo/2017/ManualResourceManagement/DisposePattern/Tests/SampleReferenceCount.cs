using System;
using System.Threading;

namespace DisposePattern.Tests
{
    /// <summary>
    /// <see cref="SampleDisopsableStruct"/>を参照カウント付きにしたもの。
    /// </summary>
    struct SampleReferenceCount : IReferenceCoutable
    {
        private Counter _counter;

        public SampleReferenceCount(Counter counter)
        {
            counter.Increment();
            _counter = counter;
            _count = new Integer();
        }

        public void Dispose()
        {
            var c = Interlocked.Exchange(ref _counter, null);
            if (c == null) throw new Exception("2重Disposeずるとまずいという想定");
            c.Decrement();
        }

        #region 参照カウント

        // 複数の変数で参照カウントを共有するために1個クラスが必要。
        // Nativeリソースとかを参照する構造体の場合、そのNative側にカウントを持ってもらう手もある。

        class Integer { public int Value; }
        private Integer _count;
        public ref int Count => ref _count.Value;

        #endregion
    }
}
