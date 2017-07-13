namespace NativeInterop.Dll
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// C# 側 string → ネイティブ側 null 終端文字列の interop の例。
    ///
    /// .NET の string 型は COM の BSTR 互換なメモリレイアウトになってる。
    /// さらに、BSTR は null 終端文字列と互換。
    /// その結果、string はコピーなしでポインターをそのままネイティブコードに渡せる。
    ///
    /// DllImport では、何らかの変換が必要な場合に限り変換処理を行って、
    /// 極力そのままブロックコピーとかポインター渡しで引数を渡してくれる。
    ///
    /// なので、
    /// - ネイティブ側が wchar_t* で受けてる → string は互換性があるのでコピーを作らない
    /// - ネイティブ側が char* で受けてる → 変換が必要。コピーが作られる
    /// という挙動になる。
    /// </summary>
    class StringSample
    {
        // FillA16, FillA8 はいずれも、引数の文字列を全文字 a に上書きする関数。

        // 対 UTF-16。無変換で(ポインター渡しで)呼び出せる。
        [DllImport("Win32Dll.dll", CharSet = CharSet.Unicode)]
        extern static void FillA16(string s);

        // 対 ASCII。変換が必要。
        [DllImport("Win32Dll.dll", CharSet = CharSet.Ansi)]
        extern static void FillA8(string s);

        public static void Main()
        {
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

            // 注意: .NET の string は通常の文脈では書き換え不能に作ってあるものの、
            // unsafe やネイティブコードを介すると普通に書き換えできる。
        }
    }
}
