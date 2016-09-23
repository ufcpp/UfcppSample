namespace Async
{
    using async = System.Threading.Tasks.Task;

    class Program
    {
        // 原理的には C# 4.0 時代にあり得るコード
        // ちゃんとコンパイル可能
        // この async は Task クラスのエイリアス
        static async F()
        {
            return async.Delay(1);
        }

        // ちゃんと、1つ目の async がキーワード、2つ目の async は型名
        static async async G()
        {
            await async.Delay(1);
        }
    }
}
