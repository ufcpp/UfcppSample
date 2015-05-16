namespace Boxing.IntAsObject
{
    using System;

    class Program
    {
        static void Main()
        {
            Write(5);
            Write("aaa");
        }

        static void Write(object x)
        {
            Console.WriteLine(x.GetType().Name + " " + x.ToString());
        }
    }
}
