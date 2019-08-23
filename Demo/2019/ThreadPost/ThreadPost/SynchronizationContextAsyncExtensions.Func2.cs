using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadPost
{
    public static partial class SynchronizationContextAsyncExtensions
    {
        public static ValueTask<TResult> PostAsync<TState, TResult>(this SynchronizationContext context, Func<TState, TResult> func, TState state)
        {
            var t = StateFuncValueTaskSource<TState, TResult>.Rent(func, state);
            context.Post(x => Unsafe.As<object, StateFuncValueTaskSource<TState, TResult>>(ref x).Invoke(), t);
            return new ValueTask<TResult>(t, t.Version);
        }

        private class StateFuncValueTaskSource<TState, TResult> : ManualResetValueTaskSource<TResult>
        {
            private ObjectPool<StateFuncValueTaskSource<TState, TResult>> Pool;
            private (Func<TState, TResult> func, TState state) State;

            public override TResult GetResult(short token)
            {
                var r = base.GetResult(token);
                Pool.Return(this);
                Pool = null;
                return r;
            }

            public void Invoke()
            {
                var (func, state) = State;
                State = default;

                try
                {
                    var r = func(state);
                    SetResult(r);
                }
                catch (Exception ex)
                {
                    SetException(ex);
                }
            }

            private static readonly ObjectPool<StateFuncValueTaskSource<TState, TResult>> _pool = new ObjectPool<StateFuncValueTaskSource<TState, TResult>>(() => new StateFuncValueTaskSource<TState, TResult>());

            public static StateFuncValueTaskSource<TState, TResult> Rent(Func<TState, TResult> func, TState state)
            {
                var r = _pool.Rent();
                r.Pool = _pool;
                r.State = (func, state);
                r.Reset();
                return r;
            }
        }
    }
}
