namespace NativeInterop.YourOwnDllImport
{
    using System;
    using System.Runtime.InteropServices;

    class Program
    {
        // 対 UTF-16。無変換で(ポインター渡しで)呼び出せる。
        [DllImport("Win32Dll.dll", CharSet = CharSet.Unicode)]
        extern static void FillA16(string s);

        // 対 ASCII。変換が必要。
        [DllImport("Win32Dll.dll", CharSet = CharSet.Ansi)]
        extern static void FillA8(string s);

        [DllImport("Win32Dll.dll")]
        extern static int GetValue();

        // ネイティブ側のシグネチャは Data Shift(Data data)
        // Data は書き struct Data と同じ構造(8バイト)
        // サイズが同じ値型なら別の型でも interop 可能
        [DllImport("Win32Dll.dll")]
        extern static ulong Shift(ulong data);

        struct Data
        {
            public byte A;
            public byte B;
            public ushort C;
            public uint D;
            public override string ToString() => $"{A:X2}{B:X2}{C:X4}{D:X8}";
        }

        [DllImport("Win32Dll.dll")]
        extern static Data Shift(Data data);

        public static void Main()
        {
            Console.WriteLine(GetValue());

            // 変換が必要な方。
            // コピーが書き換わるだけなので、s1 には影響なし。
            var s1 = "awsedrftgyhu";
            FillA8(s1);
            Console.WriteLine(s1); // awsedrftgyhu

            // ポインターで渡る方。
            // s2 はネイティブ コード側での書き換えの影響を受ける。
            var s2 = "awsedrftgyhu";
            FillA16(s2);
            Console.WriteLine(s2); // aaaaaaaaaaaa

            Data data = new Data { A = 0x01, B = 0x02, C = 0x0304, D = 0x05060708 };
            data = Shift(data);
            Console.WriteLine(data);

            ulong v = 0x0102030405060708;
            v = Shift(v);
            Console.WriteLine(v.ToString("X16"));

            Console.ReadKey();
        }
    }
}
