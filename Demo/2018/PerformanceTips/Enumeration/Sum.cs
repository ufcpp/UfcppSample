using System.Collections.Generic;

namespace Enumeration
{
    class Sum
    {
        public static int SumArray(ListLike<int> list)
        {
            var sum = 0;
            var (array, length) = (list.Array, list.Length);
            for (int i = 0; i < length; i++)
                sum += array[i];
            return sum;
        }

        public static int SumSpan(ListLike<int> list)
        {
            // Fast Span (.NET Core 2.1 以降限定)だけど、foreach で普通に生列挙と同じスピードが出るのが Span のいいところ。
            var sum = 0;
            foreach (var x in list.GetSpan())
                sum += x;
            return sum;
        }

        public static int SumEnumerable(ListLike<int> list)
        {
            var sum = 0;
            // foreach (var x in list) で行けるんだけど、比較のために生利用
            var e = list.GetEnumerator();
            while (e.MoveNext())
                sum += e.Current;
            return sum;
        }

        public static int SumFastEnumerable(ListLike<int> list)
        {
            var sum = 0;
            var e = list.GetFastEnumerator();
            var x = e.TryMoveNext(out var success);
            while (success)
            {
                sum += x;
                x = e.TryMoveNext(out success);
            }
            return sum;
        }

        public static int SumEnumeratorInterface(IEnumerator<int> e)
        {
            var sum = 0;
            while (e.MoveNext())
                sum += e.Current;
            return sum;
        }

        public static int SumFastEnumeratorInterface(IFastEnumerator<int> e)
        {
            var sum = 0;
            var x = e.TryMoveNext(out var success);
            while (success)
            {
                sum += x;
                x = e.TryMoveNext(out success);
            }
            return sum;
        }
    }
}
