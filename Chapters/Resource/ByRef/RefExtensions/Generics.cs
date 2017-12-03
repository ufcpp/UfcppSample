using System;

namespace ByRef.RefExtensions.Generics
{
    static class Extensions
    {
        // 構造体(値型)は OK
        public static void M(ref this int x) { }
        public static void MI(in this int x) { }

#if InvalidCode
        // クラス(参照型)はダメ。コンパイル エラー
        public static void M(ref this string x) { }

        // 制約が付いていないとダメ。コンパイル エラー
        public static void M1<T>(ref this T x) { }
#endif

        // ref の場合、struct 制約が付いていれば OK
        public static void M2<T>(ref this T x) where T : struct { }

        // in の場合、struct 制約が付いてもダメ
#if InvalidCode
        public static void M3<T>(in this T x) where T : struct { }
#endif
    }

#if InvalidCode
    static class ReasonWhyClassNotAllowed
    {
        // (もしもこれをコンパイル エラーにしなかった場合)
        public static void M(ref this string s)
        {
            // 拡張メソッドの中で参照を書き換える
            s = null;
        }

        static void Main()
        {
            var s = "abc";
            s.M(); // M の中で s = null される
            Console.WriteLine(s); // null になってる
        }
    }

    static class ReasonWhyGenericStructNotAllowed
    {
        // (もしもこれをコンパイル エラーにしなかった場合)
        public static void M<T>(in this T s)
            where T : IDisposable
        {
            // 結局、この Dispose 呼び出しのところでコピーが起こる
            // コピーを避けるためには T が readonly struct でないとダメ
            // インターフェイス越しなので readonly struct かどうかの判定が不可能
            s.Dispose();
        }
    }
#endif
}
