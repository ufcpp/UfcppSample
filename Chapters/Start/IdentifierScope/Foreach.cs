using System;
using System.Collections.Generic;
using System.Linq;

namespace IdentifierScope.Foreach
{
    public class Program
    {
        public static void M(IEnumerable<int> list)
        {
            foreach (var i in list)
            {
                Console.WriteLine(i);
            }

            {
                // C# 3.0 以前
                var e = list.GetEnumerator();
                using (e as IDisposable)
                {
                    int i; // ループの外
                    while (e.MoveNext())
                    {
                        i = e.Current;
                        Console.WriteLine(i);
                    }
                }
            }

            {
                // C# 4.0 以降
                var e = list.GetEnumerator();
                using (e as IDisposable)
                {
                    while (e.MoveNext())
                    {
                        var i = e.Current; // ループの中
                        Console.WriteLine(i);
                    }
                }
            }
        }

        public static void M1()
        {
            Action a = null;

            foreach (var i in Enumerable.Range(0, 10))
            {
                // C# 3.0 以前: この i はずっと共有
                // C# 4.0 以降: この i は1回1回別
                a += () => Console.WriteLine(i);
            }

            // C# 3.0 以前: 9が10回表示される
            // C# 4.0 以降: 0～9が1回ずつ表示される
            a();
        }
    }
}
