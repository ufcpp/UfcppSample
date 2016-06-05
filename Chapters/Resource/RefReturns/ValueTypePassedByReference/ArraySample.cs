using System.Collections.Generic;

namespace RefReturns.ValueTypePassedByReference
{
    class ArraySample
    {
        public static void Main()
        {
            var array = new[]
            {
                new Point(),
                new Point(),
            };
            // 配列のインデクサーは要素への参照になってる
            // 値型の要素の書き換え可能
            array[0].X = 1; // OK

            var list = new List<Point>
            {
                new Point(),
                new Point(),
            };
            // これまで、ユーザー定義のインデクサーは参照返せなかった
            // 当然、C# 6以前からあるクラスのインデクサーは値型の要素の書き換え不能
#if false
            list[0].X = 1; // コンパイル エラー
#endif
        }
    }
}
