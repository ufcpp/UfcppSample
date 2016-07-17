namespace LocalFunctions.Iterator3
{
    using System;
    using System.Collections.Generic;

    static class MyEnumerable
    {
        public static IEnumerable<T> Where<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            // イテレーターではなくなった(イテレーターなのは WhereInternal の方)ので、ちゃんと呼ばれた時点でチェックが走る
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            IEnumerable<T> f()
            {
                foreach (var x in source)
                    if (predicate(x))
                        yield return x;
            }

            return f();
        }
    }
}
