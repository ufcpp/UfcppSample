using System;

/// <summary>
/// 数学関数を作ってみる例。
/// </summary>
class MyMath
{
    public static void Test()
    {
        var r = new Random();

        // 10桁程度の制度で一致してたらよしとする
        const double epsilon = 1e-10;

        for (int i = 0; i < 20; i++)
        {
            var x = 0.1 * r.NextDouble();

            // C# 標準の Sin 関数と値を比べてみる。
            var y1 = Sin(x);
            var y2 = Math.Sin(x);

            if(y1 - y2 > epsilon)
                Console.WriteLine("値が一致しませんでした");
        }
    }

    /// <summary>
    /// sin x を求める関数。
    /// テイラー展開を利用。
    /// かなり適当に作ってるので、この方法ではそんなに精度はよくない。
    /// とはいえ、10桁程度なら C# 標準の Sin 関数と値が一致するはず。
    /// </summary>
    static double Sin(double x)
    {
        double xx = -x * x;
        double fact = 1;
        double sin = x;
        for (int i = 1; i < 100;)
        {
            fact *= i; ++i; fact *= i; ++i;
            x *= xx;
            sin += x / fact;
        }
        return sin;
    }
}
