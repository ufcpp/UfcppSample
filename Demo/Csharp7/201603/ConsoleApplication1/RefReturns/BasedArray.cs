namespace ConsoleApplication1.RefReturns.BasedArray
{
    /// <summary>
    /// 開始インデックスを任意に指定できる配列(いわゆる non-zero based 配列)。
    /// 参照戻り値版。
    /// </summary>
    /// <typeparam name="T">配列の要素の型</typeparam>
    struct RefArray<T>
    {
        private readonly T[] _data;
        public int BaseIndex { get; }
        public RefArray(int baseIndex, int capacity) { BaseIndex = baseIndex; _data = new T[capacity]; }

        public ref T this[int i] => ref _data[i - BaseIndex];
    }

    /// <summary>
    /// 開始インデックスを任意に指定できる配列(いわゆる non-zero based 配列)。
    /// get/set版。
    /// </summary>
    /// <typeparam name="T">配列の要素の型</typeparam>
    struct GetSetArray<T>
    {
        // この3行は RefArray と一緒。
        private readonly T[] _data;
        public int BaseIndex { get; }
        public GetSetArray(int baseIndex, int capacity) { BaseIndex = baseIndex; _data = new T[capacity]; }

        public T this[int i]
        {
            get { return _data[i - BaseIndex]; }
            set { _data[i - BaseIndex] = value; }
        }
    }

    class Program
    {
        static void Main()
        {
            const int N = 3;
            var a0 = new Point[N];
            var a1 = new RefArray<Point>(0, N);
            var a2 = new GetSetArray<Point>(0, N);

            a0[0].X = 1; // OK。配列のインデクサーは実は参照になってる
            a1[0].X = 1; // OK。配列と同列！

            //a2[0].X = 1;

            // ↑これだとエラー。
            // getの戻り値(コピー品)を書き換えようとしてるので、右辺値書き換え(やっちゃダメ)になる
            // ↓こう書かないとダメ。

            var p = a2[0];
            p.X = 1;       // X だけ(4バイト)書き換えてるように見えて
            a2[0] = p;     // Point 全体(12バイト)の読み書きが発生

            System.Console.WriteLine(a0[0]);
            System.Console.WriteLine(a1[0]);
            System.Console.WriteLine(a2[0]);
        }
    }
}
