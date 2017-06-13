using System;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        const int N = 30;
        const int Wait = 30;

        static void Main(string[] args)
        {
            Interop.SetCallback(Callback);

            for (int i = 0; i < N; i++)
            {
                Interop.Trigger(i);
            }

            Thread.Sleep(Wait * (N + 1));
        }

        private static async void Callback(int senderId, IntPtr data, IntPtr dataLen)
        {
            // data の寿命は、素の状態だとこのコールバック内のみ。

            // なので、ここで一時配列を取って、Buffer.MemoryCopy とかでデータをコピーしてから使うとかもよくある手法なんだけども…
            // やっぱ一時配列の確保とかコピーを避けたい。
            // 特に、data がかなり巨大だとコピーのコストが馬鹿にならないので。

            try
            {
                // 参照カウントを増やして、コールバックから外に出ても data の指す先を残してもらう。
                Interop.AddRef(data);

                // data に対して時間が掛かる処理をしたり、という前提。
                // 例えば data に巨大な JSON が入っていて、それをデシリアライズするのにそれなりの時間かかるとかいう状況で、別スレッドでデシリアライズ処理したいとか。
                await Task.Delay(senderId * Wait);

                // await の後ろなのでここはもうコールバックの外。
                // AddRef したので data はまだ生きてる。

                Console.WriteLine("sender: " + senderId);
                Console.Write("data: ");
                WriteBytes(data, dataLen);
                Console.WriteLine();
            }
            finally
            {
                // 処理が終わったら参照カウントを減らす。
                Interop.Release(data);
            }
        }

        static unsafe void WriteBytes(IntPtr data, IntPtr dataLen)
        {
            var s = new Span<byte>((byte*)data, (int)dataLen);

            for (int i = 0; i < s.Length; i++)
            {
                Console.Write("{0:X2}", s[i]);
            }
        }
    }
}
