using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApp1._02_1_ThrowExpression
{
    // throw を式中に書けるようになった (これまではステートメント)
    // ただし、書けるのは以下の3つの状況のみ。
    //   ?? の2項目
    //   ?: の2、3項目
    //   => 直後

    class _02
    {
        // 値を返すときは => が使えたのに
        public int GetValue() => 1;
        // 例外でも => が使える
        public void Never() => throw new InvalidOperationException();

        public static void Run()
        {
            // ラムダ式でも同様
            Action a = () => throw new InvalidOperationException();
        }

        static int X(int? x) => x ?? throw new ArgumentNullException(nameof(x), "my error message");

        static int ParsePositive(string s)
            => int.TryParse(s, out var x) && x > 0
            ? x
            : throw new ArgumentOutOfRangeException();

        // 将来的に switch 相当の式も入る予定で、それとの組み合わせも有効
#if false
        public static int Switch(object x)
            => x match (
                case int n => n,
                case int[] a => a.Sum(),
                default => throw new IndexOutOfRangeException()
            );
#endif
    }
}
