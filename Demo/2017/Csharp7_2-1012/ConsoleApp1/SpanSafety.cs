//#define InvalidCode

// System.Span<T> 構造体は System.Memory パッケージ内に定義されてる

using System;

namespace SpanSafety
{
    class Program
    {
        static void Main()
        {
            byte x = 0;
            ref var y = ref ValidRef(ref x);

            var a = new byte[] { 1, 2, 3 };
            var b = ValidSpan(a);
        }

        #region C# 7.0 で参照戻り値追加

        // 引数由来の参照を戻り値に返すのはOK
        static ref byte ValidRef(ref byte x) => ref x;

#if InvalidCode
        // ローカル由来の参照を戻り値に返すのはNG
        static ref byte InvalidRef()
        {
            byte x = 1;
            return ref x;
        }
#endif

        #endregion C# 7.2

        // 参照に類する型の扱いが出来るように
        // ぶっちゃけ、Span<T> 構造体のために導入されたもの
        // Span<T> は中身に「ref フィールド」的なものを持っているので、ref と同じ扱いをしないといけない
        // 「Span<T> を持つ構造体」も、再帰的に同様の扱いにする必要あり

        // 引数由来の参照を戻り値に返すのはOK
        static Span<byte> ValidSpan(Span<byte> x) => x.Slice(1);

#if InvalidCode
        // ローカル由来の参照を戻り値に返すのはNG
        static Span<byte> InvalidSpan()
        {
            Span<byte> x = stackalloc byte[10];
            return x;
        }
#endif

        #region MyRegion

        #endregion
    }
}
