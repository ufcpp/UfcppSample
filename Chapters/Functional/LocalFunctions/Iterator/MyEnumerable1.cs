namespace LocalFunctions.Iterator1
{
    using System;
    using System.Collections.Generic;

    static class MyEnumerable
    {
        public static IEnumerable<T> Where<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            // イテレーター中のコードは、最初に列挙した(foreach などに渡す)時に初めて実行される
            // このメソッドを呼んだ時点では、↓この引数チェックが働かない
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            foreach (var x in source)
                if (predicate(x))
                    yield return x;
        }
    }
}
