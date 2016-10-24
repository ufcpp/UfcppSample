namespace IdentifierScope.Iterator
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    class Sample
    {
        ~Sample()
        {
            Console.WriteLine("SampleがGCされました");
        }
    }

    public class Program
    {
        public static void M()
        {
            foreach (var i in Iterator()) ;
            AsyncMethod().Wait();
        }

        static IEnumerable<int> Iterator()
        {
            var s = new Sample();
            yield return 1;
            Console.WriteLine("1");

            // s はずっと生き残ってる。回収されない
            GC.Collect();

            yield return 2;
            Console.WriteLine("2");

            // 同上。回収されない
            GC.Collect();

            yield return 3;
            Console.WriteLine("3");
        }

        static async Task AsyncMethod()
        {
            var s = new Sample();
            await Task.Delay(1);
            Console.WriteLine("1");

            // s はずっと生き残ってる。回収されない
            GC.Collect();

            await Task.Delay(1);
            Console.WriteLine("2");

            // 同上。回収されない
            GC.Collect();

            await Task.Delay(1);
            Console.WriteLine("3");
        }
    }
}
