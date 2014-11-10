using System;
using System.Text;

namespace VersionSample.Csharp3
{
    public class QueryExpressionSample
    {
        public static void Run()
        {
            var output =
                from x in new QueryLog<int>()
                where true
                select new QueryLog<string>();

            var theSameAsAbove = new QueryLog<int>()
                .Where(x => true)
                .Select(x => new QueryLog<string>());

            Console.WriteLine(output);
        }
    }

    public delegate QueryLog<TResult> Selector<TSource, TResult>(QueryLog<TSource> item);
    public delegate bool Predicate<TSource>(QueryLog<TSource> item);

    public struct QueryLog<T>
    {

        private StringBuilder _log;

        public QueryLog<TResult> Select<TResult>(Selector<T, TResult> selector)
        {
            _log = _log ?? new StringBuilder();
            _log.AppendFormat(".Select<{0}, {1}>", typeof(T).Name, typeof(TResult).Name);
            var x = selector(this);
            x._log = _log;
            return x;
        }

        public QueryLog<T> Where(Predicate<T> predicate)
        {
            _log = _log ?? new StringBuilder();
            _log.AppendFormat(".Where<{0}>", typeof(T).Name);
            return this;
        }

        public override string ToString()
        {
            return _log == null ? "" :  _log.ToString();
        }
    }
}
