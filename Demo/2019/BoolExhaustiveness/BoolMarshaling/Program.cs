namespace BoolMarshaling
{
    using System;
    using System.Runtime.InteropServices;

    class Program
    {
        static void Main(string[] args)
        {
            // 素通し。当然、2。
            byte a = Id(2);
            Console.WriteLine(a);

            // 素通しじゃなくて、bool で値を受け取り。true。
            bool b = ToBool(2);
            Console.WriteLine(b);

            unsafe
            {
                // 内部表現を見てみると、1 になってる。
                byte b1 = *(byte*)&b;
                Console.WriteLine(b1);
            }
        }

        /// <summary>
        /// rust 側の id 関数は i8 を素通しするだけ。
        /// それを DllImport で呼んでるので、このメソッドも素通し。
        /// </summary>
        [DllImport("lib.dll", EntryPoint = "id")]
        private static extern byte Id(byte x);

        /// <summary>
        /// マーシャリングで、byte な戻り値を bool で受け取ることができる。
        /// ただ、この場合、素通しではなくて、ちゃんと 戻り値 != 0 で bool に変換されているみたい。
        /// </summary>
        [DllImport("lib.dll", EntryPoint = "id")]
        private static extern bool ToBool(byte x);
    }
}
