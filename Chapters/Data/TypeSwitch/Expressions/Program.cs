namespace TypeSwitch.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    class Program
    {
        static void Main()
        {
            var x = Node.X;

            var expressions = new[]
            {
                x * x + 1,
                2 * x + 3,
                x * x + 2 * x + 1,
                5 * x * x * x * x * x + 4 * x * x * x * x + 3 * x * x * x + 2 * x * x + 1,
            };
            var values = new[] { 1, 2, 3, 5, 7, 11 };

            WriteResults(expressions, values);

            const int N = 100000;
            var sw = new Stopwatch();

            sw.Start();
            for (int i = 0; i < N; i++)
                foreach (var ex in expressions)
                    foreach (var v in values)
                        ex.Calculate(v);
            sw.Stop();
            Console.WriteLine("virtual method: " + sw.Elapsed);

            sw.Reset();

            sw.Start();
            for (int i = 0; i < N; i++)
                foreach (var ex in expressions)
                    foreach (var v in values)
                        NodeExtensions.Calculate(ex, v);
            sw.Stop();
            Console.WriteLine("type switch   : " + sw.Elapsed);
        }

        private static void WriteResults(Node[] expressions, int[] values)
        {
            foreach (var ex in expressions)
            {
                foreach (var v in values)
                {
                    var r = ex.Calculate(v);
                    Console.WriteLine($"{ex.ToCsharpCode()} --[x = {v}]-> {r}");
                }
            }
        }
    }
}
