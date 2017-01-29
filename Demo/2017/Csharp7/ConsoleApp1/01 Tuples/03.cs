using System;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleApp1._01_Tuples
{
    class _03
    {
        public static async void Run()
        {
            var data = Enumerable.Range(0, 10000).ToArray();

            var (count, sum) = await Tally(data);

            Console.WriteLine($"{count} {sum}");
        }

        private static async Task<(int count, int sum)> Tally(int[] data)
        {
            await Task.Delay(1);

            var count = 0;
            var sum = 0;
            foreach (var x in data)
            {
                count++;
                sum += x;
            }

            return (count, sum);
        }
    }
}
