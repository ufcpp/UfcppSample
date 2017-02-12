using System.Threading.Tasks;

namespace ConsoleApp1._05_GeneralizedAsync
{
    class _01
    {
        static async Task<int> LoadAsync()
        {
            await Task.Delay(500); // ほんとは通信か何かしてる
            return 123;
        }

        // 初回だけ通信して、後はローカルにキャッシュした値を読みたい
        static Task<int> Cache => _cache ?? (_cache = LoadAsync());
        static Task<int> _cache;

        // キャッシュに依存した何らかの値をとりだす
        static async Task<int> GetValue(int x) => x * (await Cache);

        // なんども GetValue を呼ぶ
        public static async Task Run()
        {
            for (int x = 0; x < 100000; x++)
            {
                var y = await GetValue(x);
            }
        }
    }
}
