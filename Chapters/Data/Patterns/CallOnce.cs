using System;
using System.Collections.Generic;
using System.Text;

namespace Patterns.CallOnce
{
    class X
    {
        public int Value { get; }
        public X(int value) => Value = value;
        public void Deconstruct(out int value) => value = Value;
    }

    class Program
    {
        static int M(X x)
            => x switch
        {
            // 引数の数が同じ位置パターンを3回。
            // この場合、Deconstruct(out int) の呼び出しは1回にまとめられる。
            (0) _ => 1,
            (1) _ => 2,
            (2) _ => 0,
            _ => x.Value
        };
    }
}
