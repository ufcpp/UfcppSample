using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NativeInterop.Dll
{
    /// <summary>
    /// ネイティブ側とのやり取りにコールバック関数を使いたい場合。
    /// .NET のデリゲートを、ネイティブ側の関数ポインターにマーシャリングしてくれる仕組みがある。
    /// </summary>
    class CallbackSample
    {
        delegate void Callback(IntPtr param, uint value);

        [DllImport("Win32Dll.dll")]
        extern static void SetCallback(IntPtr param, Callback callback);

        [DllImport("Win32Dll.dll")]
        extern static void FireCallback(uint value);

        public static void Main()
        {
            StaticMethod();
            //NonReferencedInstanceMethod(); // こいつは呼ぶと実行時エラー
            ReferencedInstanceMethod();
            ViaCallbackParameter();
        }

        /// <summary>
        /// ガベコレを誘発用に無駄にインスタンスを量産。
        /// </summary>
        private static void MakeGarbage()
        {
            for (int i = 0; i < 1000; i++)
            {
                var x = new byte[1000];
            }
        }

        /// <summary>
        /// static メソッドをコールバックとして渡す。
        /// 問題なし。
        /// </summary>
        private static void StaticMethod()
        {
            void SetHandler()
            {
                // 何のインスタンスも参照しないメソッドであれば、問題は特に怒らない。
                // GC が起ころうと、消えないし動かない。
                SetCallback((IntPtr)0, (param, value) => Console.WriteLine(value));
            }

            MakeGarbage();

            SetHandler();

            MakeGarbage();
            GC.Collect(2, GCCollectionMode.Forced);

            for (uint i = 0; i < 5; i++)
                FireCallback(i);
        }

        /// <summary>
        /// インスタンスメソッドをコールバックとして渡す。
        /// そのインスタンスが GC 回収されてしまうとまずい。
        /// この例では、エラーが起きる。
        /// </summary>
        private static void NonReferencedInstanceMethod()
        {
            void SetHandler()
            {
                // クロージャにしてしまったので、インスタンスが作られて、ラムダ式もインスタンスメソッドになる。
                // かつ、そのインスタンスはこのローカル関数を抜けた時点でGC回収の対象になってしまう。
                uint sum = 0;
                SetCallback((IntPtr)0, (param, value) => Console.WriteLine(sum += value));
            }

            MakeGarbage();

            SetHandler();

            MakeGarbage();
            GC.Collect(2, GCCollectionMode.Forced);
            // ここで、上記ラムダ式はGC回収されてしまう。

            for (uint i = 0; i < 5; i++)
                FireCallback(i); // ここで回収済みのインスタンスに触ろうとして、実行時エラーが起きる。
        }

        /// <summary>
        /// <see cref="NonReferencedInstanceMethod"/> の問題を回避するためのコード。
        /// </summary>
        private static void ReferencedInstanceMethod()
        {
            Callback SetHandler()
            {
                // NonReferencedInstanceMethod との差は、登録したハンドラーを戻り値で返して、呼び出し側で握り続けてもらうこと。
                uint sum = 0;
                Callback c = (param, value) => Console.WriteLine(sum += value);
                SetCallback((IntPtr)0, c);
                return c;
            }

            MakeGarbage();

            // ただ握ってるだけ。特に使わなくてもいいんでとにかく変数に受ける。
            var callback = SetHandler();

            MakeGarbage();
            GC.Collect(2, GCCollectionMode.Forced);
            // GC は怒ってるので、Managed なデリゲートはコンパクションで場所が移動してる。

            // それでもエラーにならなくなる。
            // CLR 内部の挙動としては、
            // - デリゲートを呼び出してくれる static な関数を用意してある
            // - その static な関数は、コンパクションによる移動はトラッキングしてくれる
            // - でも、デリゲートは GC の対象にはなる(弱参照的な挙動)
            // みたい。
            for (uint i = 0; i < 5; i++)
                FireCallback(i);
        }

        /// <summary>
        /// <see cref="ReferencedInstanceMethod"/> のような挙動もちょっと特殊と言えば特殊で。
        /// ネイティブに渡すコールバックは static で済むなら static がいい感じもあり。
        ///
        /// 今回の場合、<see cref="SetCallback(IntPtr, Callback)"/> の第1引数に渡したパラメーターを、
        /// <see cref="Callback"/> の第1引数で渡してもらえる作りになってる。
        /// ここに、インスタンスを渡せないかを考える。
        ///
        /// ネイティブ側を経由するので、pinned ポインターを使う必要がある。
        /// <seealso cref="GCHandle.Alloc(object, GCHandleType)"/>
        /// <seealso cref="GCHandleType.Pinned"/>
        /// <seealso cref="GCHandle.AddrOfPinnedObject"/>
        /// </summary>
        private static void ViaCallbackParameter()
        {
            // コールバックに渡してほしいパラメーターを用意
            var p = new CallbackParameter();

            // ネイティブに渡しても大丈夫なように pinned (ピン止め)して、
            var h = GCHandle.Alloc(p, GCHandleType.Pinned);
            // IntPtr 化する。
            var ptr = GCHandle.ToIntPtr(h);

            void SetHandler()
            {
                // IntPtr 化したオブジェクト(のアドレス)
                SetCallback(ptr, (param, value) =>
                {
                    // ポインターからオブジェクトを復元
                    var p1 = (CallbackParameter)GCHandle.FromIntPtr(param).Target;
                    Console.WriteLine(p1.Value += value);
                });
            }

            MakeGarbage();

            SetHandler();

            MakeGarbage();
            GC.Collect(2, GCCollectionMode.Forced);
            // コールバックは static なのでガベコレの影響を受けない。
            // h.Free() しない限りは p もコンパクションの影響を受けない。

            for (uint i = 0; i < 5; i++)
                FireCallback(i);

            // ただし、Free を忘れるとメモリリークする。
            h.Free();
        }

        // Sequential にしておかないと GCHandle.Alloc できない。
        [StructLayout(LayoutKind.Sequential)]
        class CallbackParameter
        {
            public uint Value;
        }
    }
}
