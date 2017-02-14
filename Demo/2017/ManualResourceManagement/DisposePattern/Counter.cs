using System;
using System.Threading;

namespace DisposePattern
{
    /// <summary>
    /// 生成されているインスタンス数の現在値、過去の最大値をカウントするためのクラス。
    /// </summary>
    class Counter
    {
        public int _current;

        public int Current => _current;
        public int Max { get; private set; }

        public void Increment()
        {
            Interlocked.Increment(ref _current);
            Max = Math.Max(Max, _current);
        }

        public void Decrement() => Interlocked.Decrement(ref _current);
    }
}
