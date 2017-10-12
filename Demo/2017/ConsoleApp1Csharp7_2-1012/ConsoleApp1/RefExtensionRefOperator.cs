using System;

namespace RefExtensionRefOperator
{
    struct LargeStruct
    {
        public double A11;
        public double A12;
        public double A13;
        public double A21;
        public double A22;
        public double A23;
        public double A31;
        public double A32;
        public double A33;

        public LargeStruct(double a11, double a12, double a13, double a21, double a22, double a23, double a31, double a32, double a33)
        {
            A11 = a11;
            A12 = a12;
            A13 = a13;
            A21 = a21;
            A22 = a22;
            A23 = a23;
            A31 = a31;
            A32 = a32;
            A33 = a33;
        }

        // 演算子の引数に ref readonly が使えるように
        // この ref readonly は、たぶん、正式リリースまでに「in キーワードを使え」に変更される
        public static LargeStruct operator +(ref readonly LargeStruct x, ref readonly LargeStruct y) => new LargeStruct(
            x.A11 + y.A11,
            x.A12 + y.A12,
            x.A13 + y.A13,
            x.A21 + y.A21,
            x.A22 + y.A22,
            x.A23 + y.A23,
            x.A31 + y.A31,
            x.A32 + y.A32,
            x.A33 + y.A33);

        public override string ToString() => $"[({A11}, {A12}, {A13}) / ({A21}, {A22}, {A23}) / ({A31}, {A32}, {A33})]";
    }

    static class Ex
    {
        // ref this で、拡張メソッドに参照を渡せるように
        public static void Transpose(ref this LargeStruct x)
        {
            (x.A12, x.A21) = (x.A21, x.A12);
            (x.A23, x.A32) = (x.A32, x.A23);
            (x.A31, x.A13) = (x.A13, x.A31);
        }

        // 同上、ref readonly this で読み取り専用に
        public static double Trace(ref readonly this LargeStruct x)
        {
#if InvalidCode
            x.A11 = 1; // 書き換え不可
#endif
            return x.A11 + x.A22 + x.A33;
        }

#if InvalidCode
        // ちなみに、ref this が使えるのは値型のみ
        // 参照型に対してやるのは禁止(コンパイル エラー)
        public static int X(ref this string s) => s.Length;
#endif
    }

    class Program
    {
        static void Main()
        {
            var x = new LargeStruct(1, 2, 3, 4, 5, 6, 7, 8, 9);
            var y = new LargeStruct(0, -1, -2, 0, -1, -2, 0, -1, -2);

            Console.WriteLine(x);     // [(1, 2, 3) / (4, 5, 6) / (7, 8, 9)]

            // 演算子に対して ref readonly で渡る = コピーのコストがなくなる(※引数のみ。戻り値は相変わらずコピー)
            Console.WriteLine(x + y); // [(1, 1, 1) / (4, 4, 4) / (7, 7, 7)]

            // 拡張メソッドだけど、x がちゃんと書き換わる
            x.Transpose();
            Console.WriteLine(x);     // [(1, 4, 7) / (2, 5, 8) / (3, 6, 9)]

            // 拡張メソッドだけど、コピーの発生を避けれる & ちゃんと readonly なので x が書き換わらない保証あり
            Console.WriteLine(x.Trace()); // 15
        }
    }
}
