using System;

namespace NonTrailingNamedArguments
{
    class Program
    {
        static void Main()
        {
            // C# 4.0
            // 名前付きにできるのは後ろの方だけ
            X(1, 2, z: 3);
            X(1, z: 3, y: 2);
            X(x: 1, y: 2, z: 3);

            // C# 7.2
            // 末尾以外でも名前を書けるように
            X(x: 1, 2, 3);

#if InvalidCode
        // C# 7.2 でもダメなやつ
        // 末尾以外に名前付き引数を持ってきた場合、順序は厳守する必要あり
        X(2, 3, x: 1);
#endif
        }

        static void X(int x = 0, int y = 0, int z = 0) => Console.WriteLine((x, y, z));
    }
}
