using System;
using System.Collections.Generic;
using System.Text;

namespace VS16_1_p2.ReadOnlyRef
{
    struct S
    {
        public int[] _value;

        // これは、読み取り専用参照を返すという意味。
        // _value 配列の中身が書き換わってもらっては困る。
        public ref readonly int X => ref _value[0];

        // これは、S 内のフィールド(この場合 _value) を書き換えないという意味。
        // _value 配列の中身が書き換わろうと知ったことではない。
        public readonly ref int Y => ref _value[0];

        // これは、上記2つの両方の意味。
        // _value 自体も書き換わらないし、_value の中身を書き換えてもらっても困るとき用。
        public readonly ref readonly int Z => ref _value[0];
    }

    class Program
    {
    }
}
