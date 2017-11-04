namespace RecursiveReadOnly.StructReadonly
{
    using System;

    // 書き換え可能なクラス
    struct MutableStruct
    {
        // フィールドを直接公開
        public int X;

        // フィールドの値を書き換えるメソッド
        public void M(int value) => X = value;
    }

    class Program
    {
        static readonly MutableStruct c = new MutableStruct();

        static void Main() => Allowed();

#if InvalidCode
        private static void NotAllowed()
        {
            // これはもちろん許されない。c は readonly なので、c 自体の書き換えはできない
            c = new MutableStruct();

            // 構造体の場合、フィールドに関しては readonly な性質を引き継ぐ
            c.X = 1;
        }
#endif

        private static void Allowed()
        {
            // でも、メソッドは呼べてしまう
            c.M(3); // X を 3 で上書きしているはず？

            Console.WriteLine(c.X); // でも、X は 0 のまま

            //↑のコードは、実はコピーが発生している
            // 以下のコードと同じ意味なる

            var local = c;
            local.M(3);

            Console.WriteLine(c.X); // 書き換わってるのは local (コピー)の方なので、c は書き換わらない(0)

            Console.WriteLine(local.X); // もちろんこっちは書き換わってる(3)
        }
    }
}
