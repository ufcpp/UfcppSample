using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TaskLibrary.Channels.Test.TestData;

namespace TaskLibrary.Channels.Test
{
    [TestClass]
    public class TestReplicableChannel
    {
        [TestMethod]
        public void 再現実行できているか()
        {
            const int NumExecuteLoop = 10;
            var initialValues = new[] { 1, 3, 5, 7, 11, 13 };

            var res1 = Run(NumExecuteLoop, null, initialValues);
            var res2 = Run(NumExecuteLoop, res1.Item1, initialValues); // 完全に再現実行
            var res3 = Run(NumExecuteLoop, res1.Item1.Take(res1.Item1.Count() / 2), initialValues); // 半分だけ

            var b1 = res1.Item1.OfType<ResponseB>().ToArray();
            var b2 = res2.Item1.OfType<ResponseB>().ToArray();
            var b3 = res3.Item1.OfType<ResponseB>().ToArray();

            Assert.IsTrue(b1.SequenceEqual(b2, new ResponseBComparer()));
            Assert.IsTrue(res1.Item2.SequenceEqual(res2.Item2));

            Assert.IsTrue(b1.SequenceEqual(b3, new ResponseBComparer()));
            Assert.IsTrue(res1.Item2.SequenceEqual(res3.Item2));
        }

        struct ResponseBComparer : IEqualityComparer<ResponseB>
        {
            public bool Equals(ResponseB x, ResponseB y) => x.Value == y.Value;
            public int GetHashCode(ResponseB obj) => obj.Value.GetHashCode();
        }

        private static ValueTuple<IEnumerable<Response>, int[]> Run(int NumExecuteLoop, IEnumerable<Response> responses, int[] initialValues)
        {
            var values = initialValues.ToArray();

            var c = new Channel<Message>();
            var reproducer = new ReplicableChannel<Message, Response>(c, responses);
            Subscribe(reproducer.ManualChannel);
            Execute(NumExecuteLoop, c, values);

            return ValueTuple.Create(reproducer.Responses.Select(x => x.Response), values);
        }

        private static void Subscribe(ISender<Holder<Message>> sender)
        {
            var d = sender.Distribute();

            for (int i = 0; i < 6; i++)
            {
                var address = i;
                d.GetChannel(address).Subscribe(new[]
                {
                    TypedAsyncAction<Message>.Create<MessageB>((m, ct) =>
                    {
                        var x = m.Value;
                        m.SetResult(x + 1);
                        return Task.CompletedTask;
                    }),
                });
            }
        }

        private static Func<CancellableReceiver<Message>, Task> Execute(int NumExecuteLoop, Channel<Message> c, int[] values)
        {
            Func<CancellableReceiver<Message>, Task> executor = async r =>
            {
                await Task.Run(() => { }).ConfigureAwait(false);

                for (int i = 0; i < NumExecuteLoop; i++)
                {
                    await r.SendAsync(new MessageA("abc"));

                    var x = (await r.SendAsync(new MessageB(0, values[0]))).GetResponse();
                    var y = (await r.SendAsync(new[]
                    {
                        new MessageB(1, values[1]),
                        new MessageB(2, values[2]),
                        new MessageB(3, values[3]),
                        new MessageB(4, values[4]),
                        new MessageB(5, values[5]),
                    })).GetResponse().ToArray();

                    values[0] = y[0].Value;
                    values[1] = y[1].Value;
                    values[2] = y[2].Value;
                    values[3] = y[3].Value;
                    values[4] = y[4].Value;
                    values[5] = x.Value;
                }
            };

            c.Execute(executor, CancellationToken.None);
            c.Completed.Wait();
            return executor;
        }
    }
}
