using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TaskLibrary.Channels.Test.TestData;

namespace TaskLibrary.Channels.Test
{
    using System.Collections.Concurrent;

    [TestClass]
    public class TestDispatcherChannel
    {
        [TestMethod]
        public void 単一スレッド動作()
        {
            const int NumExecuteLoop = 30;

            var mp = new MessagePumpSynchronizationContext();

            var c = new Channel<Message>();
            ISender<Holder<Message>> s = c.ObserveOn(mp);

            var subscriberThreadIds = new ConcurrentDictionary<int, int>();

            s.Subscribe(async (m, ct) =>
            {
                await Task.Yield();
                subscriberThreadIds.AddOrUpdate(Thread.CurrentThread.ManagedThreadId, 1, (k, v) => v + 1);
                await Task.Run(() => { });
                subscriberThreadIds.AddOrUpdate(Thread.CurrentThread.ManagedThreadId, 1, (k, v) => v + 1);
                await Task.Delay(1);
                subscriberThreadIds.AddOrUpdate(Thread.CurrentThread.ManagedThreadId, 1, (k, v) => v + 1);
            });

            var executorThreadIds = new ConcurrentDictionary<int, int>();

            Func<CancellableReceiver<Message>, Task> executor = async r =>
            {
                await Task.Run(() => { }).ConfigureAwait(false);

                for (int i = 0; i < NumExecuteLoop; i++)
                {
                    await Task.Delay(1);
                    executorThreadIds.AddOrUpdate(Thread.CurrentThread.ManagedThreadId, 1, (k, v) => v + 1);
                    await r.SendAsync(new MessageA("abc"));
                    executorThreadIds.AddOrUpdate(Thread.CurrentThread.ManagedThreadId, 1, (k, v) => v + 1);
                    var res = (await r.SendAsync(new MessageB(0, i))).GetResponse();
                    executorThreadIds.AddOrUpdate(Thread.CurrentThread.ManagedThreadId, 1, (k, v) => v + 1);
                }
            };

            c.Execute(executor, CancellationToken.None);
            c.Completed.Wait();

            Assert.IsTrue(executorThreadIds.Keys.Count() > 1);
            Assert.AreEqual(1, subscriberThreadIds.Keys.Count());

            Assert.AreEqual(mp.ManagedThreadId, subscriberThreadIds.First().Key);

            foreach (var ek in executorThreadIds.Keys)
            {
                Assert.AreNotEqual(mp.ManagedThreadId, ek);
            }

            Assert.AreEqual(2 * 3 * NumExecuteLoop, subscriberThreadIds.Sum(x => x.Value));
            Assert.AreEqual(3 * NumExecuteLoop, executorThreadIds.Sum(x => x.Value));

            mp.Stop();
        }

        [TestMethod]
        public void 同期コンテキストがnullでも動く()
        {
            const int NumExecuteLoop = 30;

            var c = new Channel<Message>();
            ISender<Holder<Message>> s = c.ObserveOn(null);

            var subscriberThreadIds = new ConcurrentDictionary<int, int>();

            s.Subscribe(async (m, ct) =>
            {
                await Task.Yield();
                subscriberThreadIds.AddOrUpdate(Thread.CurrentThread.ManagedThreadId, 1, (k, v) => v + 1);
                await Task.Run(() => { });
                subscriberThreadIds.AddOrUpdate(Thread.CurrentThread.ManagedThreadId, 1, (k, v) => v + 1);
                await Task.Delay(1);
                subscriberThreadIds.AddOrUpdate(Thread.CurrentThread.ManagedThreadId, 1, (k, v) => v + 1);
            });

            var executorThreadIds = new ConcurrentDictionary<int, int>();

            Func<CancellableReceiver<Message>, Task> executor = async r =>
            {
                await Task.Run(() => { }).ConfigureAwait(false);

                for (int i = 0; i < NumExecuteLoop; i++)
                {
                    await Task.Delay(1);
                    executorThreadIds.AddOrUpdate(Thread.CurrentThread.ManagedThreadId, 1, (k, v) => v + 1);
                    await r.SendAsync(new MessageA("abc"));
                    executorThreadIds.AddOrUpdate(Thread.CurrentThread.ManagedThreadId, 1, (k, v) => v + 1);
                    var res = (await r.SendAsync(new MessageB(0, i))).GetResponse();
                    executorThreadIds.AddOrUpdate(Thread.CurrentThread.ManagedThreadId, 1, (k, v) => v + 1);
                }
            };

            c.Execute(executor, CancellationToken.None);
            c.Completed.Wait();

            Assert.IsTrue(subscriberThreadIds.Keys.Count() > 1);
            Assert.IsTrue(executorThreadIds.Keys.Count() > 1);

            Assert.AreEqual(2 * 3 * NumExecuteLoop, subscriberThreadIds.Sum(x => x.Value));
            Assert.AreEqual(3 * NumExecuteLoop, executorThreadIds.Sum(x => x.Value));
        }

        [TestMethod]
        public void DistribusiveChannelと組み合わせ()
        {
            const int NumExecuteLoop = 30;

            var mp = new MessagePumpSynchronizationContext();

            var c = new Channel<Message>();
            ISender<Holder<Message>> s = c.ObserveOn(mp);
            var d = s.Distribute();

            var subscriberThreadIds = new ConcurrentDictionary<int, int>();

            for (int i = 0; i < 6; i++)
            {
                d.GetChannel(i).Subscribe(async (m, ct) =>
                {
                    await Task.Yield();
                    subscriberThreadIds.AddOrUpdate(Thread.CurrentThread.ManagedThreadId, 1, (k, v) => v + 1);
                    await Task.Run(() => { });
                    subscriberThreadIds.AddOrUpdate(Thread.CurrentThread.ManagedThreadId, 1, (k, v) => v + 1);
                    await Task.Delay(1);
                    subscriberThreadIds.AddOrUpdate(Thread.CurrentThread.ManagedThreadId, 1, (k, v) => v + 1);
                });
            }

            var executorThreadIds = new ConcurrentDictionary<int, int>();

            Func<CancellableReceiver<Message>, Task> executor = async r =>
            {
                await Task.Run(() => { }).ConfigureAwait(false);

                for (int i = 0; i < NumExecuteLoop; i++)
                {
                    await Task.Delay(1);
                    executorThreadIds.AddOrUpdate(Thread.CurrentThread.ManagedThreadId, 1, (k, v) => v + 1);
                    await r.SendAsync(new MessageA("abc"));
                    executorThreadIds.AddOrUpdate(Thread.CurrentThread.ManagedThreadId, 1, (k, v) => v + 1);
                    var x = (await r.SendAsync(new MessageB(0, i))).GetResponse();
                    var y = (await r.SendAsync(new[]
                    {
                        new MessageB(1, i),
                        new MessageB(2, i),
                        new MessageB(3, i),
                        new MessageB(4, i),
                        new MessageB(5, i),
                    })).GetResponse();
                    executorThreadIds.AddOrUpdate(Thread.CurrentThread.ManagedThreadId, 1, (k, v) => v + 1);
                }
            };

            c.Execute(executor, CancellationToken.None);
            c.Completed.Wait();

            Assert.IsTrue(executorThreadIds.Keys.Count() > 1);
            Assert.AreEqual(1, subscriberThreadIds.Keys.Count());

            Assert.AreEqual(mp.ManagedThreadId, subscriberThreadIds.First().Key);

            foreach (var ek in executorThreadIds.Keys)
            {
                Assert.AreNotEqual(mp.ManagedThreadId, ek);
            }

            Assert.AreEqual(6 * 3 * NumExecuteLoop, subscriberThreadIds.Sum(x => x.Value));
            Assert.AreEqual(3 * NumExecuteLoop, executorThreadIds.Sum(x => x.Value));

            mp.Stop();
        }
    }

    class MessagePumpSynchronizationContext : SynchronizationContext
    {
        CancellationTokenSource _cts;
        Thread _thread;

        public int ManagedThreadId { get; private set; }

        public MessagePumpSynchronizationContext()
        {
            _cts = new CancellationTokenSource();

            _thread = new Thread(x =>
            {
                ManagedThreadId = Thread.CurrentThread.ManagedThreadId;

                SetSynchronizationContext(this);
                var ct = (CancellationToken)x;

                while (true)
                {
                    if (_queue.Count == 0)
                    {
                        Thread.Sleep(10);
                        if (ct.IsCancellationRequested) return;
                        continue;
                    }

                    ValueTuple<SendOrPostCallback, object> v;
                    while (_queue.TryDequeue(out v))
                    {
                        v.Item1(v.Item2);
                        if (ct.IsCancellationRequested) return;
                    }
                }
            });

            _thread.Start(_cts.Token);
        }

        public void Stop()
        {
            _cts.Cancel();
            _thread.Join();
        }

        private ConcurrentQueue<ValueTuple<SendOrPostCallback, object>> _queue = new ConcurrentQueue<ValueTuple<SendOrPostCallback, object>>();

        public override void Post(SendOrPostCallback d, object state)
        {
            _queue.Enqueue(ValueTuple.Create(d, state));
        }
    }
}
