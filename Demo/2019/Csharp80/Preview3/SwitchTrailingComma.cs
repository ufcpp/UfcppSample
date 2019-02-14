namespace Preview3.SwitchTrailingComma
{
    // switch 式で、最後の1個の , が OK になった。
    // ただ、Preview 3 ではなんかバグがあるみたいで、switch 式を入力してると IntelliSense が狂るって最終的には Visual Studio が落ちる。

    class Program
    {
        static int M(int i) => i switch
        {
            0 => 1,
            1 => 2,
            2 => 0,
            _ => -1, // この、最後の1個の , が OK になった。
        };
    }
}
