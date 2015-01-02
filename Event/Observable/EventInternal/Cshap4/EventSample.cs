namespace Observable.EventInternal.Cshap4
{
    using System;
    using System.Threading;

    /// <summary>
    /// そして、C# 4.0 以降では、自動実装がこんな感じに展開される。
    /// いわゆる「lock-free アルゴリズム」っていうやつ。
    /// <see cref="Interlocked.CompareExchange(ref object, object, object)"/> を使うと lock を避けれる。
    /// lock を使うよりはだいぶ低負荷なもの。
    /// </summary>
    class EventSample
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
