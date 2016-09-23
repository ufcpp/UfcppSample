using System;

namespace Keywords.Yield
{
    class cs1
    {
        static void Calc(decimal dividends, decimal price)
        {
            // yield には歩留まりとか出来高みたいな意味があって、
            // こういう変数名を使う人がいてもおかしくはない
            decimal yield = dividends / price;
            Console.WriteLine(yield);
        }
    }
}
