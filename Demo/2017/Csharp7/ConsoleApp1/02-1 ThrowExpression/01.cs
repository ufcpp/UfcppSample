using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1._02_1_ThrowExpression
{
    // throw を式中に書けるようになった (これまではステートメント)
    // ただし、書けるのは以下の3つの状況のみ。
    //   ?? の2項目
    //   ?: の2、3項目
    //   => 直後

    // out var とかパターン マッチングとの相性がいいのでこのタイミングで紹介

    class _01
    {
        // 値を返すときは => が使えたのに
        public int GetValue() => 1;
        // これまで例外を投げたいときは {} 必須だった
        public void Never() { throw new InvalidOperationException(); }

        public static void Run()
        {
            // ラムダ式でも同様
            Action a = () => { throw new InvalidOperationException(); };
        }

        static int X(int? x)
        {
            // null チェック用の if
            if (x == null) throw new ArgumentNullException(nameof(x), "my error message");
            return (int)x;
        }

        static int ParsePositive(string s)
        {
            // TryParse + ちょっとした条件
            int x;
            if (int.TryParse(s, out x) && x > 0) return x;
            throw new ArgumentOutOfRangeException();
        }
    }
}
