using System.Threading.Tasks;

namespace ConsoleApp1._05_GeneralizedAsync
{
    class _02
    {
        static async Task<int> LoadAsync()
        {
            await Task.Delay(500); // ほんとは通信か何かしてる
            return 123;
        }

        static Task<int> Cache => _cache ?? (_cache = LoadAsync());
        static Task<int> _cache;

        // ここの戻り値をValueTaskに変える
        // 他は変更なし
        // ほとんどの場面で完了済みのものを await するときに非常に有効
        static async ValueTask<int> GetValue(int x) => x * (await Cache);

        public static async Task Run()
        {
            for (int x = 0; x < 100000; x++)
            {
                var y = await GetValue(x);
            }
        }
    }
}
