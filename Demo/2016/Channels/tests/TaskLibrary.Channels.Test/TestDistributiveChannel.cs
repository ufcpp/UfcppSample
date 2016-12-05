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
    public class TestDistributiveChannel
    {
        [TestMethod]
        public void 分解できているか()
        {
            const int NumExecuteLoop = 30;

            var c = new Channel<Message>();
            ISender<Holder<Message>> s = c;

            var d = s.Distribute();

            var list = new List<Holder<Message>>();

            int count1 = 0;
            int count2 = 0;
            int count3 = 0;

            s.Subscribe((m, ct) =>
            {
                list.Add(m);

                if (!m.IsArray) ++count1;
                else
                {
                    ++count2;
                    count3 += m.Array.Length;
                }

                return Task.CompletedTask;
            });

            var counts = new int[6];

            for (int i = 0; i < counts.Length; i++)
            {
                var address = i;
                var x = 0;
                d.GetChannel(address).Subscribe((m, ct) =>
                {
                    ++counts[address];
                    ((MessageB)m).SetResult(x);
                    ++x;
                    return Task.CompletedTask;
                });

            }

            int sumA = 0;
            int sumB = 0;

            Func<CancellableReceiver<Message>, Task> executor = async r =>
            {
                await Task.Run(() => { }).ConfigureAwait(false);

                for (int i = 0; i < NumExecuteLoop; i++)
                {
                    await Task.Run(() => { }).ConfigureAwait(false);
                    var x = (await r.SendAsync(new MessageB(0, i))).GetResponse();
                    var y = (await r.SendAsync(new[]
                    {
                        new MessageB(1, i),
                        new MessageB(2, i),
                        new MessageB(3, i),
                        new MessageB(4, i),
                        new MessageB(5, i),
                    })).GetResponse();
                    sumA += x.Value;
                    sumB += y.Sum(yi => yi.Value);
                }
            };

            c.Execute(executor, CancellationToken.None);

            c.Completed.Wait();

            Assert.AreEqual(2 * NumExecuteLoop, list.Count);
            Assert.AreEqual(NumExecuteLoop, list.Count(x => x.IsArray));
            Assert.AreEqual(NumExecuteLoop, list.Count(x => !x.IsArray));

            Assert.AreEqual(NumExecuteLoop, count1);
            Assert.AreEqual(NumExecuteLoop, count2);
            Assert.AreEqual(5 * NumExecuteLoop, count3);
            foreach (var x in counts)
            {
                Assert.AreEqual(NumExecuteLoop, x);
            }

            Assert.AreEqual(Sum1To(NumExecuteLoop - 1), sumA);
            Assert.AreEqual(5 * Sum1To(NumExecuteLoop - 1), sumB);
        }
    }
}
