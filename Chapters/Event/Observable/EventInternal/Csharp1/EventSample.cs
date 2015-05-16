namespace Observable.EventInternal.Csharp1
{
    using System;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// かつて自動実装と同じだったもの。
    /// <see cref="MethodImplOptions.Synchronized"/>を付けると、そのメソッドがスレッド安全になる。
    /// 簡単ではあるものの、実行性能上の問題があって…
    /// </summary>
    class EventSample
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

namespace Observable.EventInternal.Csharp1Lock
{
    using System;

    /// <summary>
    /// ちなみに、<see cref="System.Runtime.CompilerServices.MethodImplOptions.Synchronized"/> は、さらに、lock ステートメント相当のコードに展開されます。
    /// lock、はさらに言うと、<see cref="System.Threading.Monitor.TryEnter(object)"/> に展開。
    /// ロックを掛けるっていうのはかなり重たい処理で、できれば避けたい。
    ///
    /// <code>lock(this)</code> はやっちゃいけないコード。
    /// 外からロック獲得できちゃう。大変まずい。
    /// </summary>
    class EventSample
    {
        private EventHandler _X;

        public event EventHandler X
        {
            add
            {
                lock (this)
                {
                    _X = (EventHandler)Delegate.Combine(_X, value);
                }
            }
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

namespace Observable.EventInternal.Csharp1Monitor
{
    using System;
    using System.Threading;


    /// <summary>
    /// lock、はさらに言うと、<see cref="System.Threading.Monitor.Enter(object, ref bool)"/> に展開。
    ///
    /// lock の展開結果も、実は C# コンパイラーのバージョンによって何回か変更かかってるけども、今の実装はこう。
    /// Monitor.Enter の実行中に例外が出ることがある(<see cref="ThreadAbortException"/> のせい)とか、
    /// それを検出するためには ref で flag を渡さないとダメとか。
    /// </summary>
    class EventSample
    {
        private EventHandler _X;

        public event EventHandler X
        {
            add
            {
                bool flag = false;
                try
                {
                    Monitor.Enter(this, ref flag);
                    _X = (EventHandler)Delegate.Combine(_X, value);
                }
                finally
                {
                    if (flag)
                    {
                        Monitor.Exit(this);
                    }
                }
            }
            remove
            {
                bool flag = false;
                try
                {
                    Monitor.Enter(this, ref flag);
                    _X = (EventHandler)Delegate.Remove(_X, value);
                }
                finally
                {
                    if (flag)
                    {
                        Monitor.Exit(this);
                    }
                }
            }
        }
    }
}
