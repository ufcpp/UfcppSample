namespace Keywords.Await
{
    using System;
    using System.Threading.Tasks;

    class A
    {
        static int X()
        {
            var async = 2; // OK

            // 匿名関数の中などはまた別文脈
            // 匿名関数に async を付けているので、この中では await がキーワード
            Func<Task<int>> f = async () => { await Task.Delay(3); return async; };

            var await = 5; // OK
            return await * f().Result;
        }

#if false
        static async Task<int> XAsync()
        {
            var async = 2;
            Func<Task<int>> f = async () => { await Task.Delay(3); return async; };
            var await = 5; // コンパイル エラー。キーワード扱いなので変数名に使えない。
            return await * await f();
        }
#endif
    }
}

namespace Await
{
    using System.Threading.Tasks;

    class Program
    {
        public struct await { }

#if false
        static async Task<int> XAsync()
        {
            // async が付いたメソッド内では ↑ の await 型は使えない
            var x = new await(); // コンパイル エラー

            // どうしても使いたかったら @ を付けてエスケープ
            var y = new @await(); // これならコンパイルできる
        }
#endif
    }
}
