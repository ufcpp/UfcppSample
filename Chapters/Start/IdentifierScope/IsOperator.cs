namespace IdentifierScope.IsOperator
{
    using System;

    class Base { }
    class Derived1 : Base { public int Id => 1; }
    class Derived2 : Base { public string Name => "2"; }

    class Sample
    {
        public static void M(Base x)
        {
            //if (x is Derived1 d)
            //{
            //    // x の型が Derived1 だった場合だけ、キャスト結果が d に入る
            //    Console.WriteLine(d.Id);
            //}
            //else if (x is Derived2 d)
            //{
            //    // x の型が Derived2 だった場合だけ、キャスト結果が d に入る
            //    // d のスコープは if 直後の(条件が真の時の)ブロック内だけ
            //    // x is Derived1 d の方とこっちの d は別物
            //    Console.WriteLine(d.Name);
            //}
        }
    }
}
