namespace Patterns.VarPattern
{
    using System;

    class Program
    {
        static void Main()
        {
            MatchNull();
            DeclarationExpression();
        }

        static void DeclarationExpression()
        {
            while (Console.ReadLine() is var line && !string.IsNullOrEmpty(line))
            {
                Console.WriteLine(line);
            }
        }

        static void MatchNull()
        {
            string s = null;
            Console.WriteLine(s is string x); // false
            Console.WriteLine(s is var y);    // true
        }

        static int M(object x)
        {
            switch(x)
            {
                case 0: return 0;
                case string s: return s.Length;
                case var other: return other.GetHashCode();
                // あるいは、変数で受け取る必要がないときは _ にしておけば破棄の意味なる
                // case var _:
            }
        }
    }
}
