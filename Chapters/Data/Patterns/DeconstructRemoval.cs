namespace Patterns.DeconstructRemoval
{
    using System;

    class X
    {
        // Deconstruct に副作用を持たせる
        public void Deconstruct() => Console.WriteLine("Deconstruct()");
        public void Deconstruct(out int a)
        {
            Console.WriteLine("Deconstruct(out int a)");
            a = 0;
        }
        public void Deconstruct(out int a, out int b)
        {
            Console.WriteLine("Deconstruct(out int a, out int b)");
            (a, b) = (0, 0);
        }
    }

    class Program
    {
        static void Main()
        {
            var x = new X();

            // Deconstruct() がないとコンパイル エラーになるけど、
            // Deconstruct() は呼ばれない。
            Console.WriteLine(x is ());

            // Deconstruct(out int) がないとコンパイル エラーになるけど、
            // Deconstruct(out int) は呼ばれない。
            Console.WriteLine(x is var (_));

            // Deconstruct(out int, out int) がないとコンパイル エラーになるけど、
            // Deconstruct(out int, out int) は呼ばれない。
            Console.WriteLine(x is (_, _));
        }
    }
}
