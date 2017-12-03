namespace LambdaInternal.InSameBoat
{
    using System;
    using System.Threading.Tasks;

    class Program
    {
        static async Task Main()
        {
            // この2つの配列の寿命は一蓮托生になる
            var smallData = new int[5];
            var bigData = new int[10000];

            // 小さいデータしか握っていないので長寿でもそこまで問題のないデリゲート
            Func<int, int> f1 = i => smallData[i];

            // 大きめのデータを握っていて、長寿だと問題の出るデリゲート
            Func<int, int> f2 = i => bigData[i];

            // f1, f2 を使う何か
            f1(0);
            f2(0);

            // f2 の寿命が長いと問題なので用が済み次第消す
            f2 = null;

            await Task.Delay(TimeSpan.FromHours(10));

            // f1 は後で使いたい
            // f1 が生きている限り、f2 を消しても結局 bigData は残る
            f1(0);
        }
    }
}
