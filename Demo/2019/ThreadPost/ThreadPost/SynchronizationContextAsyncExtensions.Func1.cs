using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadPost
{
    public static partial class SynchronizationContextAsyncExtensions
    {
        public static ValueTask<TResult> PostAsync<TResult>(this SynchronizationContext context, Func<TResult> func)
        {
            var t = FuncValueTaskSource<TResult>.Rent(func);
            context.Post(x => Unsafe.As<object, FuncValueTaskSource<TResult>>(ref x).Invoke(), t);
            return new ValueTask<TResult>(t, t.Version);
        }

        private class FuncValueTaskSource<TResult> : ManualResetValueTaskSource<TResult>
        {
            private ObjectPool<FuncValueTaskSource<TResult>> Pool;
            private Func<TResult> State;

            public override TResult GetResult(short token)
            {
                var r = base.GetResult(token);
                Pool.Return(this);
                Pool = null;
                return r;
            }

            public void Invoke()
            {
                var func = State;
                State = default;

                try
                {
                    var r = func();
                    SetResult(r);
                }
                catch (Exception ex)
                {
                    SetException(ex);
                }
            }

            private static readonly ObjectPool<FuncValueTaskSource<TResult>> _pool = new ObjectPool<FuncValueTaskSource<TResult>>(() => new FuncValueTaskSource<TResult>());

            public static FuncValueTaskSource<TResult> Rent(Func<TResult> func)
            {
                var r = _pool.Rent();
                r.Pool = _pool;
                r.State = func;
                r.Reset();
                return r;
            }
        }
    }
}
