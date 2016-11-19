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

            Console.ReadKey();
        }
    }
}
