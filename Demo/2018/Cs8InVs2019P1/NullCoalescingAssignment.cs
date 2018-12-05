namespace Cs8InVs2019P1.NullCoalescingAssignment
{
    using System;

    class Program
    {
        static void Main()
        {
            NullCoalescingAssignment("abc"); // "abc" が表示される
            NullCoalescingAssignment(null);  // "default string" が表示される
        }

        static void NullCoalescingAssignment(string s)
        {
            s ??= "default string";
            Console.WriteLine(s);
        }
    }
}
