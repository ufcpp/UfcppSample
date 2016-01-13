using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinqSample.InputOutput
{
    class Program
    {
        public static void Run()
        {
            Write(Read()
                .Where(x => (x % 2) == 1)
                .Select(x => x * x)
                );

            Write(Filter(Read()));
            HardCoded();
        }

        static IEnumerable<int> Read()
        {
            while (true)
            {
                var line = Console.ReadLine();
                if (string.IsNullOrEmpty(line)) break;
                yield return int.Parse(line);
            }
        }

        static IEnumerable<int> Filter(IEnumerable<int> source)
        {
            foreach (var x in source)
                if ((x % 2) == 1)
                    yield return x * x;
        }

        static void Write(IEnumerable<int> source)
        {
            foreach (var x in source)
                Console.WriteLine(x);
        }

        static void HardCoded()
        {
            while (true)
            {
                var line = Console.ReadLine();
                if (string.IsNullOrEmpty(line)) break;
                var x = int.Parse(line);

                if ((x % 2) == 1)
                {
                    var y = x * x;
                    Console.WriteLine(y);
                }
            }
        }
    }
}
