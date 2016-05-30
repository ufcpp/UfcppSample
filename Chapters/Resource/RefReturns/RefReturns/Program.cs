namespace RefReturns.RefReturns
{
    using System;

    class Program
    {
        public static void Main()
        {
            var x = new[] { -1, -1, -1, -1, -1 };

            for (int i = 0; i < x.Length; i++)
            {
                // 戻り値を書き換えてる
                // 実際書き換わってるのは参照先の配列 x の i 番目
                Ref(x, i) = i;
            }

            // ↑のループで書き換えたので、結果は 0, 1, 2, 3, 4
            Console.WriteLine(string.Join(", ", x));
        }

        // 配列の i 番目の要素を参照
        static ref int Ref(int[] array, int i) => ref array[i];
    }
}
