using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TaskLibrary.Channels.Test.TestData;

namespace TaskLibrary.Channels.Test
{
    using static Helper;

    [TestClass]
    public class TestChannel
    {
        [TestMethod]
        public void Channelの基本動作()
        {
            const int NumExecuteLoop = 30;

            var c = new Channel<Message>();
            ISender<Holder<Message>> s = c;

            var list = new List<Holder<Message>>();

            int count = 0;

            s.Subscribe((m, ct) =>
            {
                list.Add(m);

                if(!m.IsArray)
                {
                    var b = m.Value as MessageB;
                    if (b != null)
                    {
                        ++count;
                        b.SetResult(count);
                    }
                }

                return Task.CompletedTask;
            });

            int sum = 0;

            Func<CancellableReceiver<Message>, Task> executor = async r =>
            {
                await Task.Run(() => { }).ConfigureAwait(false);

                for (int i = 0; i < NumExecuteLoop; i++)
                {
                    await Task.Delay(1);
                    await r.SendAsync(new MessageA("abc"));
                    var res = (await r.SendAsync(new MessageB(0, i))).GetResponse();
                    sum += res.Value;
                }
            };

            c.Execute(executor, CancellationToken.None);
            c.Completed.Wait();

            Assert.AreEqual(2 * NumExecuteLoop, list.Count);
            Assert.AreEqual(NumExecuteLoop, count);
            Assert.AreEqual(Sum1To(NumExecuteLoop), sum);

            for (int i = 0; i < 10; i++)
            {
                var a = list[2 * i];
                Assert.IsFalse(a.IsArray);
                Assert.IsTrue(a.Value is MessageA);

                var b = list[2 * i + 1];
                Assert.IsFalse(b.IsArray);
                Assert.IsTrue(b.Value is MessageB);
                Assert.AreEqual(i, ((MessageB)b.Value).Value);
            }
        }

        [TestMethod]
        public void Subscribeの同時実行が正しく動いているか()
        {
            const int NumThreads = 20;
            const int NumSubscribeLoop = 100;
            const int NumExecuteLoop = 10;

            var c = new Channel<Message>();

            int count = 0;

            Task.WhenAll(Enumerable.Range(0, NumThreads).Select(async i =>
            {
                await Task.Delay(1).ConfigureAwait(false);

                for (int j = 0; j < NumSubscribeLoop; j++)
                {
                    c.Subscribe((m, ct) =>
                    {
                        Interlocked.Increment(ref count);
                        return Task.CompletedTask;
                    });
                }
            })).Wait();

            Func<CancellableReceiver<Message>, Task> executor = async r =>
            {
                await Task.Run(() => { }).ConfigureAwait(false);

                for (int i = 0; i < NumExecuteLoop; i++)
                {
                    await Task.Delay(1);
                    await r.SendAsync(new MessageA("abc"));
                }
            };

            c.Execute(executor, CancellationToken.None);
            c.Completed.Wait();

            Assert.AreEqual(NumExecuteLoop * NumSubscribeLoop * NumThreads, count);
        }
    }
}
