using System;
using System.Runtime.InteropServices;
using HeapAllocation.Data;
using HeapAllocation.Allocators;

namespace HeapAllocation
{
    /// <summary>
    /// 小さくて寿命が短いオブジェクトを大量に作ってみて性能を見てみる。
    /// </summary>
    public class Allocation
    {
        /// <summary>
        /// 参考実装。
        /// 構造体にするだけでどれくらい速いか。
        ///
        /// 当たり前だけどダントツで速い。
        /// クラスを構造体に変えただけで、<see cref="GarbageCollection"/>より余裕で1桁速い。
        /// 小さくて頻繁に new して寿命が短いオブジェクトは構造体にしないといけないというのがよくわかる例。
        /// </summary>
        public static (int x, int y) Struct(int loops)
        {
            var p = new StructPoint(1, 2);
            for (int i = 0; i < loops; i++)
            {
                p = new StructPoint(p.Y, p.X + p.Y);
            }
            return (p.X, p.Y);
        }

        /// <summary>
        /// .NET の GC に任せる。
        /// こんなコード、通常は構造体を使ってやるけども、そこをあえてクラスにするとどのくらいの負担か見てみる。
        ///
        /// <see cref="Struct(int)"/>の7倍くらいの遅さ。
        /// スタック(構造体利用)とヒープ(クラス利用)で1桁も差が付かないのは相当速い部類。
        /// 一般に、Mark and Sweep でGC管理されてるヒープは確保のコスト低い。
        /// </summary>
        public static (int x, int y) GarbageCollection(int loops)
        {
            var p = new ClassPoint(1, 2);
            for (int i = 0; i < loops; i++)
            {
                p = new ClassPoint(p.Y, p.X + p.Y);
            }
            return (p.X, p.Y);
        }

        /// <summary>
        /// malloc (msvcrt.dll の P/Invoke)でメモリ確保してみる例。
        /// ベタ書きな参考実装版。
        /// <seealso cref="Malloc(int)"/>
        ///
        /// 尋常じゃなく遅い。
        /// (当たり前だけど、通常、こんな頻度でGC管理外メモリ確保しない。)
        /// <see cref="GarbageCollection(int)"/>より20倍くらい遅い。
        ///
        /// tcmallocとか、msvcrtのmallocよりも10倍くらい速いものもあるらしいけども。
        /// tcmallocは内部的にGC的なことをしてるらしい。
        /// 参考: http://pages.cs.wisc.edu/~danb/google-perftools-0.98/tcmalloc.html
        /// </summary>
        public unsafe static (int x, int y) Malloc0(int loops)
        {
            var p = (StructPoint*)Interop.malloc(sizeof(StructPoint));
            p->X = 1;
            p->Y = 2;

            for (int i = 0; i < loops; i++)
            {
                var q = (StructPoint*)Interop.malloc(sizeof(StructPoint));
                q->X = p->Y;
                q->Y = p->X + p->Y;

                Interop.free((IntPtr)p);
                p = q;
            }
            var t = (p->X, p->Y);
            Interop.free((IntPtr)p);
            return t;
        }

        /// <summary>
        /// 基本的にやってることは<see cref="Malloc0(int)"/>と一緒。
        /// 煩雑な処理を<see cref="IAllocator"/>に閉じ込めただけ。
        /// パフォーマンス的には<see cref="Malloc0(int)"/> + ちょっとしたオーバーヘッド(誤差程度)になるはず。
        /// もちろん、<see cref="Malloc0(int)"/>と同様、.NETのGC基準で20倍遅い。
        /// </summary>
        public static (int x, int y) Malloc(int loops) => Pointer(MallocAllocator.Instance, loops);

        /// <summary>
        /// 自前でメモリ プールを作って、その中からメモリを払い出し・返却する実装。
        /// lock 版。
        ///
        /// こういう実装ならGC発生は全くしないし、<see cref="Marshal.AllocHGlobal(int)"/>呼び出しも最初の1回だけ。
        /// じゃあ、<see cref="GarbageCollection(int)"/>より速くなるかというと、実際計ってみると15～20倍くらい遅い。
        /// <see cref="GarbageCollection(int)"/>の方は、手元の環境だとGCが1000回の実行当たり平均55回程度発生してるけど、それでもマネージ ヒープの方が速い。
        ///
        /// 差が出る理由
        /// - lock がかなりきつい
        /// - 検索アルゴリズムの問題: 空いてるメモリ領域を探すのに、自作では並大抵のGC実装に勝てない
        /// - 最近のGCはバックグラウンドGCになっててマルチコア環境だとほとんどコストが見えない
        /// </summary>
        public static (int x, int y) LockPoolPointer(int loops) => Pointer(LockPool.Instance, loops);

        /// <summary>
        /// <see cref="LockPoolPointer(int)"/> CAS 版。
        ///
        /// lock はもともと結構重たいものなので、いわゆる lock-free アルゴリズムって言うやつで置き換え。
        /// <see cref="LockPoolPointer(int)"/>と比べると倍以上速い。
        /// それでもまだ<see cref="GarbageCollection(int)"/>より5～6倍遅い。
        /// </summary>
        public static (int x, int y) CasPoolPointer(int loops) => Pointer(CasPool.Instance, loops);

        /// <summary>
        /// <see cref="LockPoolPointer(int)"/> のスレッド ローカル版。
        ///
        /// マルチスレッド動作はあきらめる。
        /// 用途がかなり限られるものの、GCに頼らないメモリ管理としてはオーバーヘッド極小のはず。
        /// ここまで削ってまだ<see cref="GarbageCollection(int)"/>より2～3倍遅い。
        /// Mark and Sweep はほんとに速い。
        ///
        /// これが Mark and Sweep より遅くなってそうな要因は、
        /// - プールを準備する部分が重い
        /// - 検索アルゴリズムの問題
        /// - 一時変数 q が増えてるのでこれが最適化を阻害
        /// - Delete 呼び出しのコスト
        /// というあたり。
        /// </summary>
        public static (int x, int y) LocalPoolPointer(int loops)
        {
            using (var pool = new LocalPool(20))
                return Pointer(new LocalPool(20), loops);
        }

        static (int x, int y) Pointer<Allocator>(Allocator pool, int loops)
            where Allocator : IAllocator
        {
            var p = pool.New(1, 2);
            for (int i = 0; i < loops; i++)
            {
                var q = pool.New(p.Y, p.X + p.Y);
                pool.Delete(p);
                p = q;
            }
            var t = (p.X, p.Y);
            pool.Delete(p);
            return t;
        }
    }
}
