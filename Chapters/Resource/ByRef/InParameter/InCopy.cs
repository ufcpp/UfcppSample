namespace ByRef.InParameter.InCopy
{
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
        // in を付けたので readonly 扱い → M を呼ぶ際にコピー発生
        static void F(in NoReadOnly x) => x.M();

        // readonly struct であれば問題なし(コピー回避)
        static void F(in ReadOnly x) => x.M();
    }
}
