namespace LocalFunctions.TaskCache2
{
    using System;
    using System.Net.Http;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    class Program
    {
        static void Main()
        {
            MainAsync().Wait();
        }

        static async Task MainAsync()
        {
            // 何度か呼ぶけども、キャッシュされているので通信は1回きり
            Console.WriteLine(await LoadAsync());
            Console.WriteLine(await LoadAsync());
            Console.WriteLine(await LoadAsync());
        }

        static Task<string> LoadAsync()
        {
            async Task<string> inner()
            {
                var c = new HttpClient();
                var res = await c.GetAsync("http://ufcpp.net");
                var content = await res.Content.ReadAsStringAsync();

                return Regex.Match(content, @"\<title\>(.*?)\<").Groups[1].Value;
            }

            _loadCache = _loadCache ?? inner();
            return _loadCache;
        }
        static Task<string> _loadCache;
    }
}
