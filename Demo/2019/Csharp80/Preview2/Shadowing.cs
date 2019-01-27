namespace Preview2.Shadowing
{
    class Program
    {
        static void Main()
        {
            // ローカル関数内の引数・変数、その上位スコープと同じ名前を付けれるようになったみたい。
            // (上位スコープのやつを隠しちゃうので、shadowing って言う。)
            var (x, y) = (1, 2);

            // C# 7.3 までは、この引数とか変数の x, y でエラーになってた
            int a(int x, int y) => x + y;

            // キャプチャした変数なのかシャドーイングした変数なのかで紛らわしいので、
            // 使うなら static ローカル関数(キャプチャできない)とセットで使うべきかも。
            static int b() { int x = 0; return x; }

            a(x, y);
            b();
        }
    }
}
