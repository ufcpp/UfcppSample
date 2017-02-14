using System;
using System.Threading;

namespace DisposePattern.Tests
{
    /// <summary>
    /// <see cref="SampleDisopsable"/>とやっていることは一緒。
    /// <see cref="Counter"/>を1個だけ持つような小さい型に対して参照型(class)は使いたくないからstructに変えましたというもの。
    /// </summary>
    /// <remarks>
    /// この手の class → struct への変更は、結構パフォーマンス向上に効き目がある。
    /// その一方で、構造体だとデストラクターを使えないので、Dispose忘れが即リソース解放漏れになる。
    /// </remarks>
    struct SampleDisopsableStruct : IDisposable
    {
        private Counter _counter;

        public SampleDisopsableStruct(Counter counter)
        {
            counter.Increment();
            _counter = counter;
        }

        public void Dispose()
        {
            var c = Interlocked.Exchange(ref _counter, null);
            if (c == null) throw new Exception("2重Disposeずるとまずいという想定");
            c.Decrement();
        }
    }
}
