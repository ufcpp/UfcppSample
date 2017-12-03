#pragma warning disable 219

namespace StructLayoutSample.IllegalLayout
{
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Explicit)]
    struct Sample
    {
        [FieldOffset(0)]
        public int A;

        // 値と参照を同じ場所にレイアウト
        // コンパイル エラーにはならない
        [FieldOffset(0)]
        public object B;
    }

    class Program
    {
        static unsafe void Main()
        {
            // Sample 型に触れた瞬間、実行時エラーになる
            var s = new Sample();
        }
    }
}
