using System;

namespace ConsoleApp1._04_Ref02
{
    // - いくつかのアルゴリズムで、参照先を条件分岐で切り替えたい場面がある

    struct Vector3
    {
        public int X;
        public int Y;
        public int Z;
    }

    static class VectorExtensions
    {
        // ベクトル計算時に、X, Y, Z 座標を 0, 1, 2 のインデックスで読み書きしたいことがある
        // 読み書き共に頻繁なら、Get, Set を用意するよりも参照を返せるほうが高パフォーマンス
        public static ref int Ref(ref Vector3 v, int index)
        {
            switch (index)
            {
                case 0: return ref v.X;
                case 1: return ref v.Y;
                case 2: return ref v.Z;
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}
