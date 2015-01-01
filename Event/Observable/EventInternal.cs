namespace Observable
{
    using System;

    /// <summary>
    /// 自動実装。
    /// </summary>
    class EventInternal
    {
        public event EventHandler X;
    }
}

namespace Observable
{
    using System;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// かつて自動実装と同じだったもの。
    /// <see cref="MethodImplOptions.Synchronized"/>を付けると、そのメソッドがスレッド安全になる。
    /// 簡単ではあるものの、実行性能上の問題があって…
    /// </summary>
    class UsedToBeSameAsEventInternal
    {
        private EventHandler _X;

        public event EventHandler X
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            add
            {
                _X = (EventHandler)Delegate.Combine(_X, value);
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            remove
            {
                _X = (EventHandler)Delegate.Remove(_X, value);
            }
        }
    }
}

namespace Observable
{
    using System;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// ちなみに、<see cref="System.Runtime.CompilerServices.MethodImplOptions.Synchronized"/> は、さらに、lock ステートメント相当のコードに展開されます。
    /// lock、はさらに言うと、<see cref="System.Threading.Monitor.TryEnter(object)"/> に展開。
    /// ロックを掛けるっていうのはかなり重たい処理で、できれば避けたい。
    ///
    /// <code>lock(this)</code> はやっちゃいけないコード。
    /// 外からロック獲得できちゃう。大変まずい。
    /// </summary>
    class UsedToBeSameAsSynchronizedEventInternal
    {
        private EventHandler _X;

        public event EventHandler X
        {
            add
            {
                lock(this)
                {
                    _X = (EventHandler)Delegate.Combine(_X, value);
                }
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            remove
            {
                lock (this)
                {
                    _X = (EventHandler)Delegate.Remove(_X, value);
                }
            }
        }
    }
}

namespace Observable
{
    using System;
    using System.Threading;

    /// <summary>
    /// そして、C# 4.0 以降では、自動実装がこんな感じに展開される。
    /// いわゆる「lock-free アルゴリズム」っていうやつ。
    /// <see cref="Interlocked.CompareExchange(ref object, object, object)"/> を使うと lock を避けれる。
    /// lock を使うよりはだいぶ低負荷なもの。
    /// </summary>
    class SameAsEventInternal
    {
        private EventHandler _X;

        public event EventHandler X
        {
            add
            {
                EventHandler x2;
                var x1 = _X;
                do
                {
                    x2 = x1;
                    var x3 = (EventHandler)Delegate.Combine(x2, value);
                    x1 = Interlocked.CompareExchange(ref _X, x3, x2);
                }
                while (x1 != x2);
            }
            remove
            {
                EventHandler x2;
                var x1 = _X;
                do
                {
                    x2 = x1;
                    var x3 = (EventHandler)Delegate.Remove(x2, value);
                    x1 = Interlocked.CompareExchange(ref _X, x3, x2);
                }
                while (x1 != x2);
            }
        }
    }
}
