using System;

namespace Expressions
{
    class Expressions
    {
        int _x = 10 * 1000 * 1000;

        Expressions() : this(2 * 3) { }
        Expressions(int n) { }

        static void M()
        {
            var x = 1 + 2 * 3 > 4 || 5 * 6 + 8 < 9;
            var y = Math.Max(1 + (2 + 3) + 4, 5 + 6);
        }
    }
}
