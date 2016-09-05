namespace Lock.Monitor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    class Program
    {
        static void Main()
        {
            const int ThreadNum = 20;
            const int LoopNum = 20;
            var num = 0;

            var syncObject = new object();

            Parallel.For(0, ThreadNum, i =>
            {
                for (int j = 0; j < LoopNum; j++)
                {
                    bool lockTaken = false;
                    try
                    {
                        Monitor.TryEnter(syncObject, ref lockTaken); // ロック取得

                        //↓クリティカルセクション
                        int tmp = num;
                        Thread.Sleep(1);
                        num = tmp + 1;
                        //↑クリティカルセクション
                    }
                    finally
                    {
                        if (lockTaken)
                            Monitor.Exit(syncObject); // ロック解放
                    }
                }
            });

            Console.WriteLine(num);
        }
    }
}
