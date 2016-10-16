namespace ConsoleApplication1
{
    using System;
    using UtfString.Slices;

    class ComparisonWithSystemString
    {
        public static void Run()
        {
            var utf8RawData = new byte[] { 0x7B, 0x20, 0x22, 0x6B, 0x65, 0x79, 0x22, 0x3A, 0x20, 0x22, 0x61, 0xE3, 0x81, 0x82, 0xF0, 0x9F, 0x98, 0x80, 0x22, 0x20, 0x7D };
            var utf16RawData = new char[] { '{', ' ', '"', 'k', 'e', 'y', '"', ':', ' ', '"', 'a', 'あ', (char)0xD83D, (char)0xDE00, '"', ' ', '}' };

            // string 型
            {
                // UTF-8 → UTF-16 の変換でヒープ確保が必要
                var s1 = System.Text.Encoding.UTF8.GetString(utf8RawData);

                // string 型は char[] を受け取る場合でも、内部でコピーを作るのでヒープ確保発生
                var s2 = new string(utf16RawData);

                // string.Substring もコピー発生
                var sub = s1.Substring(10, 4);

                Console.WriteLine(sub);
            }

            // Utf8String 型
            {
                // ヒープ確保しない実装
                var s = new Utf8String(utf8RawData);

                // インデックスでの文字取得はできない。s[0] は byte 単位のアクセスになる
                // コード ポイントの取り出しには CodePoints を使う
                // foreach もすべて構造体で展開されるのでヒープ確保不要
                foreach (var c in s.CodePoints)
                {
                    Console.WriteLine(c);
                }

                // Substring もコピー不要な実装になっている
                var sub = s.Substring(10, 8);

                foreach (var c in sub.CodePoints)
                {
                    Console.WriteLine(c);
                }
            }

            // string 型
            {
                // 内部でコピーしているので…
                var s1 = new string(utf16RawData);
                var s2 = new string(utf16RawData);

                // 元データを書き換えても
                utf16RawData[0] = '[';
                utf16RawData[16] = ']';

                // 影響は出ない
                Console.WriteLine(s1); // { "key": "aあ😀" }
                Console.WriteLine(s2); // { "key": "aあ😀" }
            }

            // Utf8String 型
            {
                // データを共有しているので…
                var s1 = new Utf8String(utf8RawData);
                var s2 = new Utf8String(utf8RawData);

                //98, 227, 129, 132, 240, 159, 144, 136
                // 元データを書き換えると
                utf8RawData[10] = 98;
                utf8RawData[11] = 227;
                utf8RawData[12] = 129;
                utf8RawData[13] = 132;
                utf8RawData[14] = 240;
                utf8RawData[15] = 159;
                utf8RawData[16] = 144;
                utf8RawData[17] = 136;

                // 影響がある
                Console.WriteLine(s1); // { "key": "bい🐈" }
                Console.WriteLine(s2); // { "key": "bい🐈" }
                Console.WriteLine(s1.Substring(10, 8)); // bい🐈
            }
        }
    }
}
