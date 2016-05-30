namespace RefReturns.RefLocal
{
    using System;

    class Program
    {
        public static void Main()
        {
            var a = 10;

            ref var b = ref a; // 参照ローカル変数。宣言側にも、値を渡す側にも ref

            var c = b;         // これは普通に値渡し(コピー)。この時点の a の値 = 10 が入る
            ref var d = ref b; // さらに参照渡しで、結局 a を参照

            d = 1; // d = b = a を書き換え

            ref var e = ref Ref(ref c); // 参照戻り値越しに、c を参照
            var f = Ref(ref c);         // これは結局、値渡し(コピー)

            ++e;   // e = c を +1。元が10なので、11に
            f = 0; // f は普通に値渡しで作った新しい変数なので他に影響なし

            // 結果は 1, 1, 11, 1, 11, 0
            // a, b, d が同じ場所を参照してて 1
            // 同上、c, e が 11
            // f が 0
            Console.WriteLine(string.Join(", ", a, b, c, d, e, f));
        }

        // 引数を素通し
        static ref int Ref(ref int x) => ref x;
    }
}
