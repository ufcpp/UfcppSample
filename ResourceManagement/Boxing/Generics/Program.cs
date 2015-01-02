namespace Boxing.Generics
{
    using System;

    class Program
    {
        static void Main()
        {
            Console.WriteLine(CompareTo((IComparable)5, 6));
            Console.WriteLine(CompareTo((IComparable<int>)5, 6));
        }

        static int CompareTo(IComparable x, int value)
        {
            // IComparable.CompareTo(object) が呼ばれる。
            // value がボックス化される
            return x.CompareTo(value);
        }

        static int CompareTo(IComparable<int> x, int value)
        {
            // IComparable<int>.CompareTo(int) が呼ばれる。
            // value は int のまま渡される
            return x.CompareTo(value);
        }
    }
}
