// この手の、式の中に stackalloc 埋め込みをさして「nested contexts」って言ってるみたい。
// 16.2 Preview 1 でコンパイルできるようになってた。でも、IntelliSense 効いてない。

namespace VS16_1_p2.NestedStackalloc
{
    using System;
    using System.Threading.Tasks;

    class Program
    {
        // Span を受け取る適当なメソッドを用意。
        static int M(Span<byte> buf) => 0;

        static void M(int len)
        {
            // if の条件式中
            if (stackalloc byte[1] == stackalloc byte[1]) ;
            M(stackalloc byte[1]);

            // でもこれが今まではダメだった。
            // C# 8.0 ではコンパイルできる。
            M(len > 512 ? new byte[len] : stackalloc byte[len]);

            // こういう書き方は C# 7.3 時代からできてた。条件演算子だけ特別扱いしてたらしい。
            Span<byte> buf = len > 512 ? new byte[len] : stackalloc byte[len];
        }

        // フィールド初期化子の中でも書ける。
        int a = M(stackalloc byte[8]);

        static async Task MAsync()
        {
            // こういう入れ子の stackalloc の場合、非同期メソッド中でも書ける。
            M(stackalloc byte[1]);

            await Task.Yield();

            {
                // これは C# 8.0 でもダメ。
                // { } でくくってて(await をまたがない状態)もダメ。
                Span<byte> buf = stackalloc byte[1];
            }
        }
    }
}
