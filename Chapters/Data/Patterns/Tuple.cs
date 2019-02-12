namespace Patterns.Tuple
{
    using System.Runtime.CompilerServices;

    class X : ITuple
    {
        public object this[int index] => index;
        public int Length => 2;
        public void Deconstruct(out int a, out int b) => (a, b) = (0, 1);
    }

    class Source
    {
        public bool TupleSyntax((int a, int b) x) => x is (1, 2);
        public bool Deconstruct(X x) => x is (1, 2);
        public bool Object(object x) => x is (1, 2);
    }

    class Result
    {
        // ValueTuple の場合は直接フィールドを参照する。
        public bool TupleSyntax((int a, int b) x)
        {
            return x.a == 1 && x.b == 2;
        }

        // コンパイル時に Deconstruct メソッドが見つかる場合はそれを使って分解。
        public bool Deconstruct(X x)
        {
            x.Deconstruct(out var a, out var b);
            return a == 1 && b == 2;
        }

        // コンパイル時の解決ができない場合、ITuple を実装しているかどうかを見る。
        // Length とインデクサーを使ってマッチング。
        public bool Object(object x)
        {
            return x is ITuple t
                && t.Length == 2
                && t[0] is int a && a == 1
                && t[1] is int b && b == 1
                ;
        }
    }
}
