namespace Preview3.PatternTrailingComma
{
    class Program
    {
        static bool M((int a, int b) t)
            => t is
        {
            a: 1,
            b: 2, // この、最後の1個の , が OK になった。
        };
    }
}
