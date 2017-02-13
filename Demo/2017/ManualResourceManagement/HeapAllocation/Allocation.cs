using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Jobs;
using BenchmarkDotNet.Engines;
using System;
using System.Runtime.InteropServices;

namespace HeapAllocation
{
    /// <summary>
    /// 小さくて寿命が短いオブジェクトを大量に作ってみるベンチマーク。
    /// </summary>
    [SimpleJob(RunStrategy.Throughput)]
    public class Allocation
    {
        const int Loops = 10000;

        /// <summary>
        /// 参考実装。
        /// 構造体にするだけでどれくらい速いか。
        ///
        /// 当たり前だけどダントツで速い。
        /// クラスを構造体に変えただけで、<see cref="GarbageCollection"/>より余裕で1桁速い。
        /// 小さくて頻繁に new して寿命が短いオブジェクトは構造体にしないといけないというのがよくわかる例。
        /// </summary>
        [Benchmark]
        public static (int x, int y) Struct() => Struct(Loops);

        public static (int x, int y) Struct(int loops)
        {
            var p = new PointStruct(1, 2);
            for (int i = 0; i < loops; i++)
            {
                p = new PointStruct(p.Y, p.X + p.Y);
            }
            return (p.X, p.Y);
        }

        /// <summary>
        /// .NET の GC に任せる。
        /// 一般に、Mark and Sweep でGC管理されてるヒープは確保のコスト低い。
        /// こんなコード、通常は構造体を使ってやるけども、そこをあえてクラスにするとどのくらいの負担か見てみる。
        /// </summary>
        [Benchmark]
        public static (int x, int y) GarbageCollection() => GarbageCollection(Loops);

        public static (int x, int y) GarbageCollection(int loops)
        {
            var p = new PointClass(1, 2);
            for (int i = 0; i < loops; i++)
            {
                p = new PointClass(p.Y, p.X + p.Y);
            }
            return (p.X, p.Y);
        }

        /// <summary>
        /// <see cref="Marshal.AllocHGlobal(int)"/>(C++のnew/deleteみたいなもの)でメモリ確保してみる例。
        /// 当たり前だけど、通常、こんな頻度でGC管理外メモリ確保しない。
        ///
        /// 尋常じゃなく遅い。
        /// 遅いのわかりきってるんでループ回数を100分の1にしておく。それでもまだ<see cref="GarbageCollection"/>より1桁遅い。
        /// 要するに、<see cref="GarbageCollection"/>と3桁以上の差が付く。ほんと .NET とかのGCは速い。
        ///
        /// 参考: http://stackoverflow.com/questions/16567836/why-is-c-heap-allocation-so-slow-compared-to-javas-heap-allocation
        /// ヒープの確保って、通常はOSに対するシステムコールになってて、かなり重たい処理になる。
        /// Java とか .NET でヒープ確保が軽いのは、ランタイムが最初に大き目の領域を確保したうえで、システムコールなしでメモリの払い出ししてくれるから。
        /// </summary>
        [Benchmark]
        public unsafe static (int x, int y) AllocHGlobal() => AllocHGlobal(Loops / 100);

        public unsafe static (int x, int y) AllocHGlobal(int loops)
        {
            var p = (PointStruct*)Marshal.AllocHGlobal(sizeof(PointStruct));
            p->X = 1;
            p->Y = 2;

            for (int i = 0; i < loops; i++)
            {
                var q = (PointStruct*)Marshal.AllocHGlobal(sizeof(PointStruct));
                q->X = p->Y;
                q->Y = p->X + p->Y;

                Marshal.Release((IntPtr)p);
                p = q;
            }
            var t = (p->X, p->Y);
            Marshal.Release((IntPtr)p);
            return t;
        }

        /// <summary>
        /// 基本的にやってることは<see cref="AllocHGlobal"/>と一緒。
        /// 煩雑な処理を<see cref="PointHGlobal"/>に閉じ込めただけ。
        /// パフォーマンス的には<see cref="AllocHGlobal"/> + ちょっとしたオーバーヘッド(誤差程度)になるはず。
        /// </summary>
        [Benchmark]
        public static (int x, int y) AllocHGlobalRefactored() => AllocHGlobalRefactored(Loops / 100);

        public static (int x, int y) AllocHGlobalRefactored(int loops)
        {
            var p = PointHGlobal.New(1, 2);
            for (int i = 0; i < loops; i++)
            {
                var q = PointHGlobal.New(p.Y, p.X + p.Y);
                p.Dispose();
                p = q;
            }
            var t = (p.X, p.Y);
            p.Dispose();
            return t;
        }

        /// <summary>
        /// 自前でメモリプールを作って、その中からメモリを払い出し・返却する実装。
        /// lock 版。
        ///
        /// こういう実装ならGC発生は全くしない。
        /// じゃあ、<see cref="GarbageCollection"/>より速くなるかというと、実際計ってみると15～20倍くらい遅い。
        /// <see cref="GarbageCollection"/>の方は、手元の環境だとGCが平均55回程度発生してるけど、それでもマネージ ヒープの方が速い。
        /// 空いてるメモリ領域を探すコストがそれなりに高いのと、最近のGCがバックグラウンドGCになっててマルチコア環境だとほとんどコストになってないせい。
        /// </summary>
        [Benchmark]
        public static (int x, int y) LockMemoryPool() => LockMemoryPool(Loops);

        public static (int x, int y) LockMemoryPool(int loops)
        {
            var p = PointLockPool.New(1, 2);
            for (int i = 0; i < loops; i++)
            {
                var q = PointLockPool.New(p.Y, p.X + p.Y);
                p.Dispose();
                p = q;
            }
            var t = (p.X, p.Y);
            p.Dispose();
            return t;
        }

        /// <summary>
        /// <see cref="LockMemoryPool"/> CAS 版。
        ///
        /// lock はもともと結構重たいものなので、いわゆる lock-free アルゴリズムって言うやつで置き換え。
        /// <see cref="LockMemoryPool"/>と比べると倍くらいは速い。
        /// それでもまだ<see cref="GarbageCollection"/>より10倍遅い。
        /// Mark and Sweep はほんと速い。
        /// </summary>
        [Benchmark]
        public static (int x, int y) CasMemoryPool() => CasMemoryPool(Loops);

        public static (int x, int y) CasMemoryPool(int loops)
        {
            var p = PointCasPool.New(1, 2);
            for (int i = 0; i < loops; i++)
            {
                var q = PointCasPool.New(p.Y, p.X + p.Y);
                p.Dispose();
                p = q;
            }
            var t = (p.X, p.Y);
            p.Dispose();
            return t;
        }
    }
}
