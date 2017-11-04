namespace ByRef.RefExtensions.Generics
{
    static class Extensions
    {
        // 構造体(値型)は OK
        public static void M(ref this int x) { }

#if InvalidCode
        // クラス(参照型)はダメ。コンパイル エラー
        public static void M(ref this string x) { }

        // 制約が付いていないとダメ。コンパイル エラー
        public static void M1<T>(ref this T x) { }
#endif

        // struct 制約が付いていれば OK
        public static void M2<T>(ref this T x) where T : struct { }
    }
}
