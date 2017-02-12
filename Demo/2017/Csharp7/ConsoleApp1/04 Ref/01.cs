using System;

namespace ConsoleApp1._04_Ref01
{
    // ref を、戻り値とローカル変数でも使えるように
    // 主な用途:
    // - いくつかのアルゴリズムで、参照先を条件分岐で切り替えたい場面がある
    // - 構造体の有効活用
    // - native/unsafeコードとの連携
    // いずれにしても、パフォーマンス改善に有効

    struct Vector3
    {
        public int X;
        public int Y;
        public int Z;
    }

    static class VectorExtensions
    {
        // ベクトル計算時に、X, Y, Z 座標を 0, 1, 2 のインデックスで読み書きしたいことがある
        public static int Get(ref Vector3 v, int index)
        {
            switch (index)
            {
                case 0: return v.X;
                case 1: return v.Y;
                case 2: return v.Z;
                default: throw new ArgumentOutOfRangeException();
            }
        }

        public static void Set(ref Vector3 v, int index, int value)
        {
            switch (index)
            {
                case 0: v.X = value; break;
                case 1: v.Y = value; break;
                case 2: v.Z = value; break;
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }

    struct PolyLine
    {
        private Vector3[] _points;
        public PolyLine(int count) => _points = new Vector3[count];

        // 構造体をインデクサー越しに読み書きしたい
        // p[i].X = 10; みたいな書き方ができない
        // 配列であればできるのに
        public Vector3 this[int index]
        {
            get => _points[index];
            set => _points[index] = value;
        }
    }
}
