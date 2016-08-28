namespace Tuples.OutParams
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// 出力引数(C# 6)版
    /// </summary>
    class Out6
    {
        static void F(Point p)
        {
            // 事前に変数を用意しないといけない/var 不可
            int x, y;
            // 1個1個 out を付けないといけない
            Deconstruct(p, out x, out y);
            Console.WriteLine($"{x}, {y}");

            //非同期メソッドには使えない
        }

        // 1個1個 out を付けないといけない
        static void Deconstruct(Point p, out int x, out int y)
        {
            // 1個1個代入
            x = p.X;
            y = p.Y;
        }
    }

    /// <summary>
    /// 出力引数(C# 7)版
    /// </summary>
    class Out7
    {
        static void F(Point p)
        {
            // 変数の事前準備は不要に
            // でも1個1個 out を付けないといけない
            Deconstruct(p, out var x, out var y);
            //Console.WriteLine($"{x}, {y}");

            //非同期メソッドには相変わらず使えない
        }

        // 1個1個 out を付けないといけない
        static void Deconstruct(Point p, out int x, out int y) => (x, y) = (p.X, p.Y);
    }

    /// <summary>
    /// タプル版
    /// </summary>
    class Tuple
    {
        static async Task F(Point p)
        {
            // 1個の var で受け取れる
            var t1 = Deconstruct(p);
            Console.WriteLine($"{t1.x}, {t1.y}");

            // 何なら分解と併せればもっと書き心地よく書ける
            var (x, y) = Deconstruct(p);
            Console.WriteLine($"{x}, {y}");

            // 非同期メソッドで使えるのはタプルだけ
            var t2 = await DeconstructAsync(p);
            Console.WriteLine($"{t2.x}, {t2.y}");
        }

        static (int x, int y) Deconstruct(Point p) => (p.X, p.Y); // 1個の式で書けて楽
        static async Task<(int x, int y)> DeconstructAsync(Point p) => (p.X, p.Y);
    }

    struct Point
    {
        public int X;
        public int Y;
    }

    /// <summary>
    /// それでもやっぱり出力引数を使う場面
    /// </summary>
    class ButStillOut
    {
        // if 内で使うなら bool 1個の戻り値の方が使いやすい
        static void TryPattern()
        {
            var s = Console.ReadLine();
            if (int.TryParse(s, out var x)) Console.WriteLine(x);
        }

        // if 内で使うならタプルの方が煩雑
        static void TuplePattern()
        {
            var s = Console.ReadLine();
            var (success, x) = Parse(s);
            if (success) Console.WriteLine(x);
        }

        static (bool success, int value) Parse(string s) => int.TryParse(s, out var x) ? (true, x) : (false, 0);

        // C# 7の is を使って、int? の null チェック
        static void NullCheckPattern()
        {
            var s = Console.ReadLine();
            if (ParseOrDefault(s) is int x) Console.WriteLine(x);
        }

        static int? ParseOrDefault(string s) => int.TryParse(s, out var x) ? x : default(int?);

        // これはオーバーロード可能
        static void F(out int x, out int y) => (x, y) = (1, 2);
        static void F(out int id, out string name) => (id, name) = (1, "abc");

        // 戻り値でのオーバーロードはできない
        // コンパイル エラーに
        static (int x, int y) F() => (1, 2);
#if false
        static (int id, string name) F() => (1, "abc");
#endif
    }
}
