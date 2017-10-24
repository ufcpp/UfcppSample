//#define InvalidCode

namespace ConsoleApp1.RefReadonly
{
    class Program
    {
        static void Main()
        {
            RefReadonlyVar();
            RefCall();
        }

        private static void RefReadonlyVar()
        {
            int x = 0;

            // 参照ローカル変数を readonly にできるように
            ref readonly int rr = ref x;

#if InvalidCode
            // readonly なので、書き換えようとしたらエラーに
            rr = 1;
#endif

            // 将来的には (ref ではない) readonly int とかも追加される予定だったはずだけど
            // (もしかしたら let キーワードとか使うかも)
            // とりあえず ref readonly だけ先に入ったっぽい
        }

        // in で「参照渡しなんだけど x は書き換えれない」という意味
        // 元々 out が「戻り値として使う意図で参照渡し」という意味なので、それとの対比で in キーワードを使うことに
        //
        // 参照ローカル変数の場合は ref readonly を使うんで、引数でも ref readonly にするかどうかはちょっと迷ったみたい。結局 in に
        static void RefMethod(in int x)
        {
#if InvalidCode
            // readonly なので、書き換えようとしたらエラーに
            x = 1;
#endif
        }

        // ref T と T でオーバーロードを作れるんだけど、in T と T でも可能
        static void Overload(int x) { }
        static void Overload(in int x) { }

        private static void RefCall()
        {
            int x = 0;

            // in (ref readonly) の場合、ref 修飾なしでメソッドを呼べる
            //
            // (readonly でない) ref の場合に ref 修飾が必要なのは、メソッドの中で書き換わる可能性があるのが怖いから
            // 書き換わらないなら修飾の類は必要とされない
            RefMethod(x);

            // 一応、in 修飾を付けても OK。値渡しと参照渡しの弁別用
            RefMethod(in x);

#if InvalidCode
            // と言っても、今のバージョン(Preview 2。10/24版)ではこのコードはエラーに。たぶん、正式リリースまでには直る
            Overload(x);
#endif
            // こっちは OK
            Overload(in x);
        }
    }
}
