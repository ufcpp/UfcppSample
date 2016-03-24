using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csharp6.Misc
{
    class StringInterpolationSample
    {
        public void X()
        {
            var x = 12300;

            // ほぼ同じ意味
            Console.WriteLine(string.Format("{0,4:x}", x));
            Console.WriteLine($"{x,4:x}");

            // 書き方を忘れて、 , と : を間違えてしまうと…

            // 実行時エラー
            Console.WriteLine(string.Format("{0,x}", x));

            // コンパイル エラー
            //Console.WriteLine($"{x,x}");

            // エスケープ
            var p = new { X = 10, Y = 20 };
            Console.WriteLine($"\"{{{p.X}, {p.Y}}}\"");

            // こちらは、逐語的文字列リテラルと同じルール
            var verbatim = $@"
""
{{
{p.X}\{p.Y}
}}
""
";

            {
                var data = new[] { 1, 2, 3 };
                var s = $"{string.Join(", ", data)} => {string.Join(", ", data.Select(i => i * i))}";
                Console.WriteLine(s);
            }
            {
                //var s1 = $"p = {p == null ? "null" : p.ToString()}"; // エラー
                var s2 = $"p = {(p == null ? "null" : p.ToString())}"; // 1段 () でくくればOK
            }
        }
    }
}
