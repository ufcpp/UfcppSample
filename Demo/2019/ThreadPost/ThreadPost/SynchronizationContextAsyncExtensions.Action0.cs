using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadPost
{
    public static partial class SynchronizationContextAsyncExtensions
    {
        public static ValueTask PostAsync(this SynchronizationContext context, Action func)
        {
            var t = ActionValueTaskSource.Rent(func);
            context.Post(x => Unsafe.As<object, ActionValueTaskSource>(ref x).Invoke(), t);
            return new ValueTask(t, t.Version);
        }

        private class ActionValueTaskSource : ManualResetValueTaskSource<object>
        {
            private ObjectPool<ActionValueTaskSource> Pool;
            private Action State;

            public override object GetResult(short token)
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
                    func();
                    SetResult(null);
                }
                catch (Exception ex)
                {
                    SetException(ex);
                }
            }

            private static readonly ObjectPool<ActionValueTaskSource> _pool = new ObjectPool<ActionValueTaskSource>(() => new ActionValueTaskSource());

            public static ActionValueTaskSource Rent(Action func)
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
