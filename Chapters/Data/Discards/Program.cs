using System;
using System.ComponentModel;

class Program
{
    static void Main()
    {
        var _ = 10;
        Console.WriteLine(_); // 10 が表示される
    }

    static void Deconstruct0()
    {
        // 商と余りを計算するメソッドがあるけども、ここでは商しか要らない
        // 要らないので適当な変数 x とかで受ける
        var (q, x) = DivRem(123, 11);

        // 逆に、余りしか要らない
        // 要らないから再び適当な変数 x で受けたいけども、x はもう使ってる
        // しょうがないから x1 とかにしとくか…
        var (x1, r) = DivRem(123, 11);
    }

    static (int quotient, int remainder) DivRem(int dividend, int divisor)
        => (Math.DivRem(dividend, divisor, out var remainder), remainder);

    static void Deconstruct()
    {
        // 商と余りを計算するメソッドがあるけども、ここでは商しか要らない
        // _ を書いたところでは、値を受け取らずに無視する
        var (q, _) = DivRem(123, 11);

        // 逆に、余りしか要らない
        // また、本来「var x」とか変数宣言を書くべき場所にも _ だけを書ける
        (_, var r) = DivRem(123, 11);
    }

    static void Deconstruct1()
    {
        // _ を書いたところでは、値を受け取らずに無視する
        var (q, _) = DivRem(123, 11);

        // _ は変数にはならないので、スコープを汚さない。別の場所でも再び _ を書ける
        // また、本来「var x」とか変数宣言を書くべき場所にも _ だけを書ける
        (_, var r) = DivRem(123, 11);
    }

    static void Deconstruct2()
    {
        // 要らないので適当な変数 x とかで受ける
        var (q, x) = DivRem(123, 11);

        // 要らないと言いつつ、参照できてしまう
        Console.WriteLine(x);

        // 要らないものは _ で破棄
        var (_, r) = DivRem(123, 11);

        // 分解の中に書いた _ は変数にはならない
        // 以下の行でコンパイル エラーになる(_ は存在しない)
#if false
        Console.WriteLine(_);
#endif
    }

    static int TypeSwitch(object obj)
    {
        switch (obj)
        {
            case int[] x: return x.Length;
            case long[] x: return 2 * x.Length;
            // int でさえあれば値は問わない
            case int _: return 1;
            // 同、long
            case long _: return 2;
            case null: return 0;
            // 以下の行をコメントアウトするとエラーに
            // 今のところ、case _ は未実装(将来的に予定はあり)
            //case _:
            default: throw new ArgumentOutOfRangeException();
        }
    }

    // 欲しいのは戻り値だけであって、out 引数で受け取った値は要らない
    static bool CanParse(string s) => int.TryParse(s, out _);

#if false
    static void Subscribe(INotifyPropertyChanged source)
    {
        // 2個目の _ が「同じ名前被ってる」エラーになる
        source.PropertyChanged += (_, _) => Console.WriteLine("property changed");
    }

    static void Subscribe(INotifyPropertyChanged source)
    {
        // (予定)C# 8?
        // 1回も _ を使わなかったら破棄扱い
        source.PropertyChanged += (_, _) => { };

        // _ を使っているのでこれは引数扱い。同名の別引数は作れない
        source.PropertyChanged += (_, _1) => Console.WriteLine(_);
    }
#endif
}
