using System;
using System.Linq;

namespace ConsoleApp1._01_Tuples
{
    class _02
    {
        public static void Run()
        {
            var data = Enumerable.Range(0, 10000).ToArray();

            // deconstruction に変更
            var (count, sum) = Tally(data);

            Console.WriteLine($"{count} {sum}");
        }

        // out 引数をタプルに変更
        // 引数っぽい書き方
        private static (int count, int sum) Tally(int[] data)
        {
            var count = 0;
            var sum = 0;
            foreach (var x in data)
            {
                count++;
                sum += x;
            }

            return (count, sum);

            // 引数っぽい書き方いくつか
            // 名前付き
            return (count: 1, sum: 2);

            // 変数宣言(型の明示)
            (int count, int sum) t = (1, 2);
            return t;

            // キャスト
            return ((int count, int sum))(1, 2);
        }

        /* ちなみに、匿名型 new { count = 1, sum = 2 } みたいなのとの差
         *
         *           タプル                 匿名型
         * 主な用途  多値戻り値              部分的なメンバー抜き出し
         * 展開結果  ValueTuple構造体＋属性  クラスの生成
         * 型の種類  値型                    参照型
         * 見た目    引数の書き方に似ている   オブジェクト初期化子の書き方に似ている
         *
         * 概ね、「用途が違えば最適な実装も違う」という感じ。
         */
    }
}
