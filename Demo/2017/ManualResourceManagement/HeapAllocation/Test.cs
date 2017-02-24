using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeapAllocation
{
    /// <summary>
    /// <see cref="Allocation"/>に対するテスト。
    /// </summary>
    static class Test
    {
        /// <summary>
        /// どの実装でも結果が同じになってるかだけ確認。
        /// どの実装でも最終的な計算結果は<see cref="ValueTuple{T1, T2}"/>にして返しているので、それの一致を確認。
        /// 1個でも不一致があったら<see cref="InvalidOperationException"/>をthrow。
        /// </summary>
        public static void Run()
        {
            const int N = 50;
            for (int i = 0; i < N; i++)
            {
                var a = Allocation.Struct(i);
                if (!a.Equals(Allocation.GarbageCollection(i))) throw new InvalidOperationException();
                if (!a.Equals(Allocation.AllocHGlobal0(i))) throw new InvalidOperationException();
                if (!a.Equals(Allocation.AllocHGlobal(i))) throw new InvalidOperationException();
            }

            // プール実装だけは並列実行があっても大丈夫かの確認が必要
            Task.WhenAll(
                Parallel(() => LockMemoryPool(N))
                .Concat(Parallel(() => LocalMemoryPool(N)))
                .Concat(Parallel(() => CasMemoryPool(N)))
                ).Wait();
        }

        private static IEnumerable<Task> Parallel(Action a) => Enumerable.Range(0, 10).Select(_ => Task.Run(a));

        private static void CasMemoryPool(int N)
        {
            for (int i = 0; i < N; i++)
            {
                var a = Allocation.Struct(i);
                var b = Allocation.CasPoolPointer(i);
                if (!a.Equals(b)) throw new InvalidOperationException();
            }
        }

        private static void LocalMemoryPool(int N)
        {
            for (int i = 0; i < N; i++)
            {
                var a = Allocation.Struct(i);
                var b = Allocation.LocalPoolPointer(i);
                if (!a.Equals(b)) throw new InvalidOperationException();
            }
        }

        private static void LockMemoryPool(int N)
        {
            for (int i = 0; i < N; i++)
            {
                var a = Allocation.Struct(i);
                var b = Allocation.LockPoolPointer(i);
                if (!a.Equals(b)) throw new InvalidOperationException();
            }
        }
    }
}
