using System;

namespace ConsoleApplication1
{
    class SurrogatePair
    {
        public static void Run()
        {
            var s = "𩸽";

            // 「𩸽」はサロゲート ペアになっているので、char としては2文字になる
            foreach (var c in s)
            {
                // Surrogate が2回表示される
                Console.WriteLine(char.GetUnicodeCategory(c));
            }

            // string を受け取る GetUnicodeCategory オーバーロードなら、
            // サロゲート ペアな文字もコード ポイントにデコードして正しくカテゴリー判定できる

            // OtherLetter (漢字のカテゴリー)が表示される
            Console.WriteLine(char.GetUnicodeCategory(s, 0));
        }
    }
}
