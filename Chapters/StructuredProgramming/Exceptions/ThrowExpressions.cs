namespace Exceptions.ThrowExpressions
{
    using System;

    class Program
    {
        // 式形式のメソッドの中( => の直後)
        static void A() => throw new NotImplementedException();

        static string B(object obj)
        {
            // null 合体演算子(??)の後ろ
            var s = obj as string ?? throw new ArgumentException(nameof(obj));

            // 条件演算子(?:)の条件以外の部分
            return s.Length == 0 ? "empty" :
                s.Length < 5 ? "short" :
                throw new InvalidOperationException("too long");
        }

        static void C()
        {
            // コンパイル エラー。この文脈に throw 式は書けない
            B(throw new InvalidOperationException());
        }

        static void D()
        {
            // コンパイル エラー。null(型を持っていない)と並べると型が決まらない。
            var x = true ? null : throw new Exception();

            // コンパイル エラー。throw 式同士を並べると型が決まらない。
            var y = true ? throw new InvalidOperationException() : throw new NotSupportedException();
        }
    }
}
