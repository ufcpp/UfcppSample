using static Iostream;

class Drawback
{
    public static void M()
    {
        // C# の思想的には書かせたくないコードの例。
        // 書けてしまうように。
        _ = cout << "Hellow World!" << endl;

        // まあ、「言語的に禁止するのはやりすぎ」、「ガイドラインで非推奨にすれば十分」ということになった。
        //
        // シフト演算子の制限緩和をしなくても(いままででも)、
        // 「Path の連結に / を使う」みたいなことは出来ちゃってたわけだし。
        // (これも「ガイドライン的にはダメ」と言われてる。)
    }
}

public static class Iostream
{
    public static readonly ConsoleOut cout = new();
    public static readonly ConsoleEndLine endl = new();

    public struct ConsoleOut
    {
        public static ConsoleOut operator <<(ConsoleOut x, string value) { Console.Write(value); return x; }
        public static ConsoleOut operator <<(ConsoleOut x, ConsoleEndLine _) { Console.WriteLine(); return x; }
    }

    public struct ConsoleEndLine { }
}
