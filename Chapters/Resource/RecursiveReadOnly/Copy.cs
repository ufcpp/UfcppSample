namespace RecursiveReadOnly.Copy
{
    using System;

    // 作りとしては readonly を意図しているので、何も書き換えしない
    // でも、struct 自体には readonly が付いていない
    struct NoReadOnly
    {
        public readonly int X;
        public void M() { }
    }

    // NoReadOnly と作りは同じ
    // ちゃんと readonly struct
    readonly struct ReadOnly
    {
        public readonly int X;
        public void M() { }
    }

    class Program
    {
        static readonly NoReadOnly nro;
        static readonly ReadOnly ro;

        static void Main()
        {
            // readonly を付けなかった場合
            // フィールド参照(読み取り)は問題ない
            Console.WriteLine(nro.X);

            // メソッド呼び出しが問題。ここでコピー発生
            // (呼び出し側では、「M の中で特に何も書き換えていない」というのを知るすべがないので、防衛的にコピーが発生)
            nro.M();

            // readonly を付けた場合
            // これなら、M をそのまま呼んでも何も書き換わらない保証があるので、コピーは起きない
            ro.M();
        }

        // これも問題あり(コピー発生)
        // in を付けたので readonly 扱い → M を呼ぶ際にコピー発生
        static void F(in NoReadOnly x) => x.M();

        // こちらも、readonly struct であれば問題なし(コピー回避)
        static void F(in ReadOnly x) => x.M();
    }
}
