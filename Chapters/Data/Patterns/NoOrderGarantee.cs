namespace Patterns.NoOrderGarantee
{
    using System;

    enum Type { A, B }

    class X
    {
        public Type Type { get; }
        public X(Type type) => Type = type;

        // それぞれ Type が一致しているときだけ値を取り出せ、そうでなければ例外
        public int A => Type == Type.A ? 1 : throw new InvalidOperationException();
        public int B => Type == Type.B ? 2 : throw new InvalidOperationException();

        // 分解でタイプ判定
        public void Deconstruct(out Type t) => t = Type;
    }

    class Program
    {
        static void Main()
        {
            Console.WriteLine(M(new X(Type.A)));
            Console.WriteLine(M(new X(Type.B)));
        }

        // 以下のコードはたまたま動く可能性はあるものの、C# の言語使用としては保証がない。
        // Deconstruct よりも先にプロパティのアクセスがあると例外が出ることがある。
        static int M(X x) => x switch
        {
            (Type.A) { A: var a } => a,
            (Type.B) { B: var b } => b,
            _ => 0
        };
    }
}
