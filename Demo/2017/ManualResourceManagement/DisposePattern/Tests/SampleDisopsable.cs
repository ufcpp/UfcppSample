using System;
using System.Threading;

namespace DisposePattern.Tests
{
    /// <summary>
    /// コンストラクターでカウント +１、Dispose/デストラクターで -1 するクラス。
    /// 2重Disposeされるとそれはそれでまずいという想定で、2度目のDisposeでは例外を投げる。
    /// </summary>
    /// <remarks>
    /// 正しく Dipose されてればカウント数はそんなに増えないはずだけど、
    /// デストラクター任せにするとGC発動するまでの間カウントが溜まってしまう。
    /// かといって、2重Dispose禁止な実装なので、過剰になってもいいからDisposeしておくってわけにはいかない。
    ///
    /// このクラスは所詮ダミーで、カウントがいくら増えようと大した負担ではないけども、
    /// 実用途だと、例えば数十～数百MBのグラフィックリソースを握っていたり、限りあるネットワークリソースを握っていたりする可能性があって、
    /// カウントがあんまり増えると壊滅的にパフォーマンスを落とすはず。
    /// </remarks>
    class SampleDisopsable : IDisposable
    {
        private Counter _counter;

        public SampleDisopsable(Counter counter)
        {
            counter.Increment();
            _counter = counter;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~SampleDisopsable() => Dispose(false);

        protected virtual void Dispose(bool isDisposing)
        {
            var c = Interlocked.Exchange(ref _counter, null);
            if (c == null) throw new Exception("2重Disposeずるとまずいという想定");
            c.Decrement();
        }
    }
}
