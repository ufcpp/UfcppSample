//#define InvalidCode

using System;

namespace NonTrailingNamedArguments
{
    class Program
    {
        static void Main()
        {
            // C# 4.0
            // 名前付きにできるのは後ろの方だけ
            Sum(1, 2, z: 3);
            Sum(1, z: 3, y: 2);
            Sum(x: 1, y: 2, z: 3);

            // C# 7.2
            // 末尾以外でも名前を書けるように
            Sum(x: 1, 2, 3);

#if InvalidCode
            // C# 7.2 でもダメなやつ
            // 末尾以外に名前付き引数を持ってきた場合、順序は厳守する必要あり
            Sum(2, 3, x: 1);
#endif
        }

        static void Order()
        {
            // OK: 前の方は位置指定、後ろの方は名前指定
            Sum(1, z: 2, y: 3);

#if InvalidCode
            // コンパイル エラー: 前の方の引数を名前指定するのはダメ
            Sum(1, x: 2, y: 3);
#endif
        }

        static int Sum(int x = 0, int y = 0, int z = 0) => x + y + z;

        static void ForInstance()
        {
            var a = new[] { 1, 2, 3, 4, 5 };
            var b = new int[3];
            Array.Copy(sourceArray: a, destinationArray: b, 3);
        }
    }
}
