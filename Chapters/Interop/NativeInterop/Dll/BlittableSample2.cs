namespace NativeInterop.Dll
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// ネイティブ側で構造体を引数・戻り値にしてる場合の話。
    /// <see cref="BlittableSample1"/> での説明通り、同じサイズの値型を用意すれば何でも呼び出し可能。
    /// ネイティブ側と同じレイアウトの構造体を用意するもよし、ulong とかで受け取ってしまうのもよし。
    /// </summary>
    class BlittableSample2
    {
        // ネイティブ側のシグネチャは Data Shift(Data data)
        // Data は8バイトの構造体
        // サイズが同じ値型なら別の型でも interop 可能(ここでは ulong を利用)
        [DllImport("Win32Dll.dll")]
        extern static ulong Shift(ulong data);

        // ネイティブ側と同じレイアウトの struct を作る
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
            Data data = new Data { A = 0x01, B = 0x02, C = 0x0304, D = 0x05060708 };
            data = Shift(data);
            Console.WriteLine(data);

            ulong v = 0x0102030405060708;
            v = Shift(v);
            Console.WriteLine(v.ToString("X16"));
        }
    }
}
