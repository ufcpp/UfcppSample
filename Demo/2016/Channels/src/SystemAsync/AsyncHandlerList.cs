using System;
using System.Linq;
using System.Threading.Tasks;

namespace SystemAsync
{
    /// <summary>
    /// Rx でいう Subject : IObserver, IObservable。
    /// <see cref="IAsyncEvent{TEventArgs}"/> の実装。
    ///
    /// <see cref="AsyncHandler{TEventArgs}"/> を束ねるクラス。
    /// 1つ1つ await しないと行けないので、マルチキャスト デリゲート(デリゲートに対する += でデリゲート連結)が使えないので、Listで実装。
    /// </summary>
    /// <example>
    /// <![CDATA[
    /// public IAsyncEvent<T> SomeEvent { get { return _someEvent; } }
    /// protected Task<T> OnSomeEvent(T args) => _someEvent.InvokeAsync(this, args);
    /// private AsyncHandlerList<T> _someEvent = new AsyncHandlerList<T>();
    /// ]]>
    /// </example>
    /// <typeparam name="TEventArgs">イベント引数の型。</typeparam>
    public class AsyncHandlerList<TEventArgs> : IAsyncEvent<TEventArgs>
    {
        private AsyncHandler<TEventArgs>[] _list;
        private object _sync = new object();

        /// <summary>
        /// 1つでもハンドラーが刺さってたら true。
        /// </summary>
        public bool HasAny => _list != null && _list.Length != 0;

        /// <summary>
        /// イベントを起こす。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public async Task InvokeAsync(object sender, TEventArgs args)
        {
            // ↓これだけでいいはずなんだけども、たぶん iOS AOT で動かない。
            // var actions = System.Threading.Interlocked.Exchange(ref _list, null);
            // _sync が要らなかったら、単なる配列の薄いラッパーだし、struct にしたいんだけども。
            AsyncHandler<TEventArgs>[] actions;
            lock (_sync)
            {
                actions = _list;
            }

            if (actions != null)
                foreach (var a in actions)
                    await a(sender, args);
        }

        private void Add(AsyncHandler<TEventArgs> action)
        {
            // こっちは CompareExchage
            lock (_sync)
            {
                _list = _list == null
                    ? new[] { action }
                    : _list.Concat(new[] { action }).ToArray();
            }
        }

        private void Remove(AsyncHandler<TEventArgs> action)
        {
            lock (_sync)
            {
                _list = _list == null
                    ? new AsyncHandler<TEventArgs>[] { }
                    : _list.Where(x => x != action).ToArray();
            }
        }

        /// <summary>
        /// <see cref="IDisposable.Dispose"/>
        /// ハンドラーを全削除。
        /// </summary>
        public void Dispose()
        {
            lock (_sync)
            {
                _list = null;
            }
        }

        /// <summary>
        /// イベント購読。
        /// </summary>
        /// <param name="handler"></param>
        /// <returns></returns>
        public IDisposable Subscribe(AsyncHandler<TEventArgs> handler)
        {
            Add(handler);
            return new ActionDisposer(() => Remove(handler));
        }
    }

    internal class ActionDisposer : IDisposable
    {
        Action _onDispose;

        public ActionDisposer(Action onDispose)
        {
            if (onDispose == null)
                throw new ArgumentNullException();
            _onDispose = onDispose;
        }

        public void Dispose()
        {
            _onDispose();
        }
    }
}
