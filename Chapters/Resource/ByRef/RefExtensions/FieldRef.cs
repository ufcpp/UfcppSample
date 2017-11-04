namespace ByRef.RefExtensions.FieldRef
{
    using System;

    struct Point
    {
        public int X;
        public int Y;
        public int Z;

#if InvalidCode
        public ref int At(int index)
        {
            switch (index)
            {
                // インスタンス メソッド(プロパティ、インデクサー)では以下の ref が認められていない(コンパイル エラー)
                case 0: return ref X;
                case 1: return ref Y;
                case 2: return ref Z;
                default: throw new IndexOutOfRangeException();
            }
        }
#endif
    }

    static class PointExtensions
    {
        public static ref int At(ref this Point p, int index)
        {
            switch (index)
            {
                // インスタンス メソッド版とやっていることは同じでも、こちらは OK
                case 0: return ref p.X;
                case 1: return ref p.Y;
                case 2: return ref p.Z;
                default: throw new IndexOutOfRangeException();
            }
        }
    }
}
