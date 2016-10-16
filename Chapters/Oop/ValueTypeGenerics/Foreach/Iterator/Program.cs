namespace ValueTypeGenerics.Foreach.Iterator
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    struct Quartet : IEnumerable<int>
    {
        public int A;
        public int B;
        public int C;
        public int D;

        public Quartet(int a, int b, int c, int d) { A = a; B = b; C = c; D = d; }

        // これの戻り値がインターフェイスなのが良くない。
        // yield return から作られる enumerable/enumeartor は構造体なのに、
        // インターフェイスを介することでボックス化が発生、ヒープ確保が必要。
        public IEnumerator<int> GetEnumerator()
        {
            yield return A;
            yield return B;
            yield return C;
            yield return D;
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    class Program
    {
        static void Main()
        {
            const int N = 10000;

            {
                var q = new Quartet(1, 2, 3, 4);
                var begin = GC.GetTotalMemory(false);

                var sum = 0;
                for (int i = 0; i < N; i++)
                    foreach (var x in q) sum += x;

                var end = GC.GetTotalMemory(false);
                Console.WriteLine($"iterator: {end - begin}"); // 0 にはならない
            }

            {
                var q = new Quartet(1, 2, 3, 4);
                var begin = GC.GetTotalMemory(false);

                var sum = 0;
                for (int i = 0; i < N; i++)
                    sum += Sum(q); // インターフェイスを介した時点でボックス化発生

                var end = GC.GetTotalMemory(false);
                Console.WriteLine($"interface: {end - begin}"); // q のボックス化 + GetEnumerator のボックス化
            }
        }

        static int Sum(IEnumerable<int> items)
        {
            var sum = 0;
            foreach (var item in items) sum += item;
            return sum;
        }
    }
}
