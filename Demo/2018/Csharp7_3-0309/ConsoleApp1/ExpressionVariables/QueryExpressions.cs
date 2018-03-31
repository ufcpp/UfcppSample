using System;
using System.Linq;

namespace ConsoleApp1.ExpressionVariables
{
    class QueryExpressions
    {
        static void Main()
        {
            var q =
                from s in new[] { "a", "123", "one", "abc", "456", "two" }

                // クエリ式中で out var が使えるように。
                let table = Table.TryGetValue(s, out var x) ? x : 0

                // 宣言した変数は句(where とか select とか)内のみ。
                // (まあ、句をまたぎたければ let を使えばいいし。)
                // なので、別の句に入ったら同じ名前の変数を使っても大丈夫。
                let parsed = int.TryParse(s, out var x) ? x : 0
#if Uncompilable
                // 逆に、ここでもはもう x は使えない。
                where x == 1
#endif
                select (table, parsed);

            // ここにも x は残らないので、改めて x を使える
            foreach (var (x, y) in q)
            {
                Console.WriteLine("TryGetValue: " + x);
                Console.WriteLine("int.TryParse: " + y);
            }

            // 大半の式の場合、out var で宣言した変数はその式を越えても使える。
            // どうして「大半」に合わせなかったかと言うと、「クエリ式の各句はラムダ式みたいなもの」だから。
            //
            // 以下のように、ラムダ式は数少ない「式の外に変数が漏れない」仕様になってる。
#if Uncompilable
            Func<string, int> f = s => int.TryParse(s, out var x) ? x : 0;
            Console.WriteLine(x); // ここでエラーになる(今までも、これからも)
#endif

            // クエリ式は、以下のようにラムダ式 + メソッド呼び出しと同じようなコードに展開される。
            // クエリ式中の変数宣言が句単位なのは、この仕様を意識したもの。
            var q1 =
                from s in new[] { "one", "two", "three" }
                where Table.TryGetValue(s, out var x) && x > 2 // x のスコープは句内
                select Table.TryGetValue(s, out var x) ? x : 0; // 同上

            var q2 =
                new[] { "one", "two", "three" }
                .Where(s => Table.TryGetValue(s, out var x) && x > 2) // x のスコープはラムダ式内
                .Select(s => Table.TryGetValue(s, out var x) ? x : 0); // 同上
        }
    }
}
