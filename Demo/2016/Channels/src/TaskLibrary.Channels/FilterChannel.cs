using SystemAsync;
using System;
using System.Threading.Tasks;

namespace TaskLibrary.Channels
{
    /// <summary>
    /// Where, Select。
    /// あんまりWhereとSelectを分ける意味なさそうだったので1クラスで両方。
    /// </summary>
    /// <typeparam name="TSource">フィルター元の要素の型。</typeparam>
    /// <typeparam name="TTarget">フィルター結果の要素の型。</typeparam>
    public class FilterChannel<TSource, TTarget> : ISender<TTarget>
    {
        private readonly ISender<TSource> _inner;
        private readonly Func<TSource, bool> _predicate;
        private readonly Func<TSource, TTarget> _selector;

        /// <summary>
        /// </summary>
        /// <param name="sender">フィルター元。</param>
        /// <param name="predicate">trueを返す要素だけを残す。</param>
        /// <param name="selector">要素の変換関数。</param>
        public FilterChannel(ISender<TSource> sender, Func<TSource, bool> predicate, Func<TSource, TTarget> selector)
        {
            _inner = sender;
            _predicate = predicate;
            _selector = selector;
        }

        public Task Completed => _inner.Completed;

        public IDisposable Subscribe(AsyncAction<TTarget> handler)
            => _inner.Subscribe(async (message, ct) =>
            {
                if (_predicate(message))
                {
                    var t = _selector(message);
                    await handler(t, ct);
                }
            });
    }

    public static partial class Channel
    {
        public static ISender<TTarget> Filter<TSource, TTarget>(this ISender<TSource> source, Func<TSource, bool> predicate, Func<TSource, TTarget> selector)
            => new FilterChannel<TSource, TTarget>(source, predicate, selector);

        public static ISender<TSource> Where<TSource>(this ISender<TSource> source, Func<TSource, bool> predicate)
            => new FilterChannel<TSource, TSource>(source, predicate, x => x);

        public static ISender<TTarget> Select<TSource, TTarget>(this ISender<TSource> source, Func<TSource, TTarget> selector)
            => new FilterChannel<TSource, TTarget>(source, x => true, selector);
    }
}
