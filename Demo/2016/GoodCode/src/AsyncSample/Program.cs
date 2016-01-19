using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;

namespace AsyncSample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            MainAsync().Wait();
        }

        private static async Task MainAsync()
        {
            var c = new HttpClient();
            var res = await c.GetAsync("http://ufcpp.net");
            var content = await res.Content.ReadAsStringAsync();
            Console.WriteLine(content);
        }

        private static Task OldMainAsync()
        {
            var c = new HttpClient();
            return c.GetAsync("http://ufcpp.net")
                .ContinueWith(res => res.Result.Content.ReadAsStringAsync())
                .ContinueWith(content => Console.WriteLine(content.Result));
        }
    }
}
