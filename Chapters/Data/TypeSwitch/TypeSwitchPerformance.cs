namespace TypeSwitch.TypeSwitchPerformance
{
    using System;
    using System.Diagnostics;

    class Program
    {
        static void Main()
        {
            var sw = new Stopwatch();

            // bool 型は一番先頭 = 速い
            object t = true;
            sw.Start();
            for (int i = 0; i < 100000; i++) TypeSwitch(t);
            sw.Stop();
            Console.WriteLine("bool   " + sw.Elapsed); // かなり速いはず

            // double 型は一番末尾 = 遅い
            object d = 1.1;
            sw.Restart();
            for (int i = 0; i < 100000; i++) TypeSwitch(d);
            sw.Stop();
            Console.WriteLine("string " + sw.Elapsed); // 手元の環境では5倍くらい遅かった

            // どの case にもない型。default 句に行く
            var s = DateTime.UtcNow;
            sw.Restart();
            for (int i = 0; i < 100000; i++) TypeSwitch(s);
            sw.Stop();
            Console.WriteLine("string " + sw.Elapsed); // 一番最後まで判定するので遅い
        }

        static int TypeSwitch(object x)
        {
            switch (x)
            {
                default: return -1; // ちなみに、default 句はどこに書こうと必ず一番最後
                case bool _: return 0; // 前から順に判定ということは、bool の時が一番早い
                case sbyte _: return 1;
                case byte _: return 2;
                case short _: return 3;
                case ushort _: return 4;
                case int _: return 5;
                case uint _: return 6;
                case long _: return 7;
                case ulong _: return 8;
                case float _: return 9;
                case double _: return 10; // 逆に double の時は凄く遅い
            }
        }
    }
}
