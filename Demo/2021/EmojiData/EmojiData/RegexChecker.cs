using System;
using System.Text.RegularExpressions;

namespace EmojiData
{
    /// <summary>
    /// 単に「json で読むべき要素読めてるか」の確認用に、別途、 "image": "*****.png" 行を Regex 検索してみて数字が合うかとかやってるだけ。
    /// </summary>
    class RegexChecker
    {
        public static void CountImages(string json)
        {
            var reg = new Regex(@"""image"":""[0-9a-fA-F\-]*\.png");
            var m = reg.Matches(json);
            Console.WriteLine("reg image count: " + m.Count);
        }
    }
}
