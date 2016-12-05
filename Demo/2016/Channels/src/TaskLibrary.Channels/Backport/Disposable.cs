using System;

namespace TaskLibrary.Channels
{
    /// <summary>
    /// Reactive Extensions(Rx)にあるようなやつ。
    /// これ1個だけのためにRx依存もしたくないので。
    /// </summary>
    internal class Disposable
    {
        public static IDisposable Create(Action dispose) => new ActionDisposable(dispose);

        class ActionDisposable : IDisposable
        {
            private Action _dispose;
            public ActionDisposable(Action dispose) { _dispose = dispose; }
            public void Dispose() => _dispose();
        }
    }
}
