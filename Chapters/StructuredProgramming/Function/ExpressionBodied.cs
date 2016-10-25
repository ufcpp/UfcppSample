namespace Function.ExpressionBodied
{
    using System;

    // C# 6 時点で => を使って書ける関数メンバー
    class Csharp6
    {
        // メソッド
        int Method(int x) => x * x;

        // 演算子
        public static Csharp6 operator +(Csharp6 x) => x;

        // プロパティ(get-only)
        int X => 0;

        // インデクサー(get-only)
        int this[int index] => index;
    }

    // C# 7 で追加された => を使って書ける関数メンバー
    class Csharp7
    {
        static int x;

        // コンストラクター
        Csharp7() => x++;

        // デストラクター
        ~Csharp7() => x--;

        // プロパティ(get/set)
        int X
        {
            get => x++;
            set => x--;
        }

        // インデクサー(get/set)
        int this[int index]
        {
            get => x += index;
            set => x -= index;
        }

        // イベント(add/remove)
        event Action E
        {
            add => x++;
            remove => x--;
        }
    }
}
