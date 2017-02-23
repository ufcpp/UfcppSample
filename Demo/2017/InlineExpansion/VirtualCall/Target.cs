namespace VirtualCall
{
    interface ISample
    {
        int Value { get; }
    }

    class A : ISample
    {
        public int Value => 1;
    }

    class B : ISample
    {
        // B の方にだけ virtual を付ける
        public virtual int Value => 1;
    }

    class Target
    {
        // A, B の Value を直接呼ぶ
        public static int CallA(A x) => x.Value;
        public static int CallB(B x) => x.Value;

        // インターフェイス越しに Value を呼ぶ
        public static int CallInterface(ISample x) => x.Value;

        // ジェネリック越しに Value を呼ぶ
        public static int Call<T>(T x) where T : ISample => x.Value;
    }
}
