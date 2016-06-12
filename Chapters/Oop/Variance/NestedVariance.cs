using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Variance
{
    // 標準ライブラリの System.Func
    public delegate TResult Func<in T, out TResult>(T arg);

    delegate Func<TIn, TOut> F<in TIn, out TOut>(Func<TOut, TIn> x);

    interface INestedVariance<in TIn, out TOut>
    {
        TOut F(TIn x, Func<TOut, TIn> f);
    }

    class NestedVariance<TIn, TOut> : INestedVariance<TIn, TOut>
    {
        public TOut F(TIn x, Func<TOut, TIn> f)
        {
            // f の戻り値は、値を受け取るためにある = in
            TIn in1 = f(default(TOut));

            // f の引数にはこちらから値を渡す = out
            TOut out1 = default(TOut);
            var r = f(out1);

            // 引数から受け取る = in
            TIn in2 = x;

            // 戻り値を返す = out
            TOut out2 = default(TOut);
            return out2;
        }
    }
}
