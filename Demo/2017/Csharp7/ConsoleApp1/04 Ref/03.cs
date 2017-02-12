using System;

namespace ConsoleApp1._04_Ref03
{
    // - 構造体の有効活用

    struct Vector3
    {
        public int X;
        public int Y;
        public int Z;
    }

    struct PolyLine
    {
        private Vector3[] _points;
        public PolyLine(int count) => _points = new Vector3[count];

        // 構造体をインデクサー越しに読み書きしたい
        // 参照で返すなら、p[i].X = 10; みたいな書き方もできる
        // 配列と同様
        public ref Vector3 this[int index] => ref _points[index];
    }
}
