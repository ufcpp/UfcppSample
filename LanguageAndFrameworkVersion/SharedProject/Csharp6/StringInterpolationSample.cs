using System;
using System.Collections.Generic;

namespace VersionSample.Csharp6
{
    /// <summary>
    /// 文字列補間。
    /// <see cref="string.Format(string, object[])"/> を単純化する構文が言語に組み込まれる。
    ///
    /// 通常版(string 型の変数に代入)の場合、
    /// 内部的には <see cref="string.Format(string, object[])"/> (.NET 1.0の頃からある)を呼んでいるだけなので、古いバージョンでも当然動く。
    /// </summary>
    public class StringInterpolationSample
    {
        public static void X()
        {
            var x = 10;
            var y = 20;

            var s = $"{x}, {y}, {x :c}, {x :n}";

            Console.WriteLine(s);
        }

        public static void SameAsX()
        {
            var x = 10;
            var y = 20;

            var s = string.Format("{0}, {1}, {2 :c}, {3 :n}", x, y, x, x);

            Console.WriteLine(s);
        }

        //【注意】この構文だとカルチャー指定とかができない(IFormatProviders を与えるオーバーロードを呼べない)。
        // 正式版までには、
        // IFormattable f = $"{x}, {y]";
        // f.ToString(null, /* ここに provider を渡せる */);
        // みたいな追加の構文が入る予定。
        //
        // この機能には、System.Runtime.CompilerServices.FormattedString 型が必要になるので、.NET 4.6 (もしくは同等の FormattedString の自前実装)が必要に。
        // C# 6.0 の機能の中で、.NET 4.6/.NET Core 5を要求する唯一の機能。

#if Ver4_6 || (Ver2 && Plus)

        private static readonly IEnumerable<string> Cultures = new[] { "en-us", "fr", "zh-hk", "ja-jp" };

        public static void Y()
        {
            var x = 10;

            IFormattable f = $"{x :c}, {x :n}";

            foreach (var c in Cultures)
            {
                var s = WithCulture(f, c);
                Console.WriteLine(s);
            }
        }

        public static void SameAsY()
        {
            var x = 10;

            IFormattable f = System.Runtime.CompilerServices.FormattableStringFactory.Create("{0:c}, {1:n}", x, x);

            foreach (var c in Cultures)
            {
                var s = WithCulture(f, c);
                Console.WriteLine(s);
            }
        }

        private static string WithCulture(IFormattable f, string cultureName)
        {
            return f.ToString(null, System.Globalization.CultureInfo.CreateSpecificCulture(cultureName));
        }

#endif
    }
}
