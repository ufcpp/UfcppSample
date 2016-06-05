using System;

namespace RefReturns.ValueTypePassedByReference
{
    /// <summary>
    /// 循環バッファー。
    /// </summary>
    /// <typeparam name="T">要素の型。</typeparam>
    class CircularBuffer<T>
    {
        private int _startIndex;
        private T[] _data;

        /// <summary>
        /// 容量を指定して初期化。
        /// </summary>
        /// <param name="capacity">容量。</param>
        public CircularBuffer(int capacity)
        {
            _startIndex = 0;
            _data = new T[capacity];
        }

        /// <summary>
        /// 値を追加。
        /// 容量を超えた分は古いものから削除。
        /// </summary>
        /// <param name="item">新しい値。</param>
        public void Push(T item)
        {
            _data[_startIndex] = item;
            _startIndex++;
            if (_startIndex >= _data.Length) _startIndex = 0;
        }

        /// <summary>
        /// 先頭要素。
        /// </summary>
        public ref T Head => ref _data[_startIndex];

        /// <summary>
        /// 先頭から <paramref name="index"/> 先の要素。
        /// </summary>
        /// <param name="index">先頭からの位置。</param>
        /// <returns></returns>
        public ref T this[int index] => ref _data[(_startIndex + index) % _data.Length];
    }

    class CircularBufferSample
    {
        public static void Main()
        {
            var x = new CircularBuffer<Point>(5);

            for (int i = 0; i < 10; i++)
            {
                x.Push(new Point { X = i, Y = i * 2 });

                for (int j = 0; j < 5; j++)
                {
                    x[j].X = 3 * j;
                }

                for (int j = 0; j < 5; j++)
                {
                    Console.Write(x[j] + ", ");
                }
                Console.WriteLine();
            }
        }
    }
}
