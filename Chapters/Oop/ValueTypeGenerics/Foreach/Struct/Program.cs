namespace ValueTypeGenerics.Foreach.Struct
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

        // がちがちにパフォーマンス最適化が必要な場合、面倒だけど enumerator の手書きが必要。
        public struct Enumerator : IEnumerator<int>
        {
            Quartet _items;
            int _i;

            public Enumerator(Quartet quintet) { _items = quintet; _i = 0; }

            public int Current =>
                _i == 1 ? _items.A :
                _i == 2 ? _items.B :
                _i == 3 ? _items.C :
                _items.D;

            public bool MoveNext()
            {
                _i++;
                return _i <= 4;
            }

            object IEnumerator.Current => Current;
            public void Dispose() { }
            public void Reset() { throw new NotImplementedException(); }
        }

        // 構造体な Enumerator を返す。
        public Enumerator GetEnumerator() => new Enumerator(this);
        IEnumerator<int> IEnumerable<int>.GetEnumerator() => GetEnumerator();
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
                Console.WriteLine($"struct: {end - begin}"); // 0 って出るはず
            }

            {
                var q = new Quartet(1, 2, 3, 4);
                var begin = GC.GetTotalMemory(false);

                var sum = 0;
                for (int i = 0; i < N; i++)
                    sum += Sum(q); // ジェネリックを介せばボックス化は起きない

                var end = GC.GetTotalMemory(false);
                Console.WriteLine($"generics: {end - begin}"); // GetEnumerator のボックス化のみ
            }
        }

        static int Sum<T>(T items)
            where T : struct, IEnumerable<int>
        {
            var sum = 0;
            foreach (var item in items) sum += item;
            return sum;
        }
    }
}
