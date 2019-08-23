using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadPost
{
    public static partial class SynchronizationContextAsyncExtensions
    {
        public static ValueTask PostAsync<TState>(this SynchronizationContext context, Action<TState> func, TState state)
        {
            var t = StateActionValueTaskSource<TState>.Rent(func, state);
            context.Post(x => Unsafe.As<object, StateActionValueTaskSource<TState>>(ref x).Invoke(), t);
            return new ValueTask(t, t.Version);
        }

        private class StateActionValueTaskSource<TState> : ManualResetValueTaskSource<object>
        {
            private ObjectPool<StateActionValueTaskSource<TState>> Pool;
            private (Action<TState> func, TState state) State;

            public override object GetResult(short token)
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
                    func(state);
                    SetResult(null);
                }
                catch (Exception ex)
                {
                    SetException(ex);
                }
            }

            private static readonly ObjectPool<StateActionValueTaskSource<TState>> _pool = new ObjectPool<StateActionValueTaskSource<TState>>(() => new StateActionValueTaskSource<TState>());

            public static StateActionValueTaskSource<TState> Rent(Action<TState> func, TState state)
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
