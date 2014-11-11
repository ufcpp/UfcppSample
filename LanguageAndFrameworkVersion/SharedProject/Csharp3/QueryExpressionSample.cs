using System;
using System.Text;

namespace VersionSample.Csharp3
{
    /// <summary>
    /// クエリ式は、単に、 Select や Where っていう名前のメソッド呼び出し(拡張メソッド可)に展開されるだけ。
    /// 拡張メソッドを使ってしまうと <see cref="System.Runtime.CompilerServices.ExtensionAttribute"/> が必要になるものの、
    /// 通常のインスタンス メソッドで Select や Where を持っているなら、普通に .NET 2.0 でもクエリ式を使える。
    /// </summary>
    public class QueryExpressionSample
    {
        public static void X()
        {
            var output =
                from x in new QueryLog<int>()
                where true
                select new QueryLog<string>();

            Console.WriteLine(output);
        }

        public static void SameAsX()
        {
            var output = new QueryLog<int>()
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
