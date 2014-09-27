using System;

class Program
{
    static void Main(string[] args)
    {
        ReferenceParameters();
        NestedStructs();
    }

    /// <summary>
    /// (標準仕様内では)C# で明示的に参照を作れるのは ref/out パラメーターのみ。
    /// </summary>
    static void ReferenceParameters()
    {
        int x = 10;
        int y;

        ReferenceParameters(ref x, out y);
        Console.WriteLine(x); // 99
        Console.WriteLine(y); // 256
    }

    static void ReferenceParameters(ref int x, out int y)
    {
        Console.WriteLine(x); // 外からわたって来た時点の値。今回の場合10
        x = 99; // 呼び出し元の側の値も書き換わる
        y = 256; // 呼び出し元の側の値が書き換わる
    }

    /// <summary>
    /// 構造体 = 値型。
    /// </summary>
    struct StructThis
    {
        public int x;
        public int Y()
        {
            // 値型の this は、参照になってる
            return this.x * this.x; // この this はメソッド Y に参照が渡されてる
        }
    }

    // 構造体の入れ子
    struct A { public B b; }
    struct B { public C c; }
    struct C { public int x; }

    /// <summary>
    /// 入れ子の奥深くのフィールドの書き換えがちゃんとできるのも、これが参照で帰ってきてるから。
    /// </summary>
    static void NestedStructs()
    {
        var a = new A();
        a.b.c.x = 1;
        Console.WriteLine(a.b.c.x); // ちゃんと x が 1 に書き換わってる
    }
}
