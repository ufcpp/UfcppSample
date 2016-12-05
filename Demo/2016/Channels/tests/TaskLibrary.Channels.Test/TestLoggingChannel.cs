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
    public class TestLoggingChannel
    {

        [TestMethod]
        public void 順序保証()
        {
            const int NumExecuteLoop = 30;

            var c = new Channel<Message>(InvocationMode.Parallel);

            var list = new List<int>();

            Func<Holder<Message>, int, Task> f = async (m, offset) =>
            {
                await Task.Delay(1);
                var value = ((MessageB)m.Value).Value;
                lock (list)
                    list.Add(3 * value + offset);
            };

            ISender<Holder<Message>> s = new LoggingChannel<Holder<Message>>(c,
                (m, ct) => f(m, 0), // A
                (m, ct) => f(m, 2), // B
                InvocationMode.Parallel);

            s.Subscribe((m, ct) => f(m, 1)); // C
            s.Subscribe((m, ct) => f(m, 1));
            s.Subscribe((m, ct) => f(m, 1));

            //↑ メッセージが来るたびに、A, C, C, C, B の順で動くはず。C 3つの順序については保証なし。

            Func<CancellableReceiver<Message>, Task> executor = async r =>
            {
                await Task.Run(() => { }).ConfigureAwait(false);

                for (int i = 0; i < NumExecuteLoop; i++)
                {
                    await Task.Delay(1);
                    var res = (await r.SendAsync(new MessageB(0, i))).GetResponse();
                }
            };

            c.Execute(executor, CancellationToken.None);
            c.Completed.Wait();

            Assert.AreEqual(5 * NumExecuteLoop, list.Count);
            Assert.IsTrue(list.SequenceEqual(list.OrderBy(x => x)));
            Assert.AreEqual(NumExecuteLoop, list.Count(x => (x % 3) == 0));
            Assert.AreEqual(3 * NumExecuteLoop, list.Count(x => (x % 3) == 1));
            Assert.AreEqual(NumExecuteLoop, list.Count(x => (x % 3) == 2));
        }
    }
}
