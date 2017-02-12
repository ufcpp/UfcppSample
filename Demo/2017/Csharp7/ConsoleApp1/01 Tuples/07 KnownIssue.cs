using System.Linq;

namespace ConsoleApp1._01_Tuples
{
    class KnownIssue
    {
        public static void Run()
        {
            var data = Enumerable.Range(0, 10000).ToArray();

            // sum と count の順番間違えてる
            // 今のところ警告も何もなし(将来追加予定)
            var (sum, count) = Tally(data);
        }

        /// <summary>
        /// docコメントを書いてみると…
        /// </summary>

        // 期待するもの

        /// <param name="data"></param>
        /// <returns name="count"></returns>
        /// <returns name="sum"></returns>

        // 実際

        /// <param name="data"></param>
        /// <returns></returns>
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
        }
    }
}
