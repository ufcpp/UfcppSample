using System;

/// <summary>
/// 数値入力の例。
/// </summary>
class InputNumberSample
{
    public static void Run()
    {
        double x, y, z; // 変数を宣言。

        // x, y にユーザーの入力した値を代入。
        x = GetDouble("input x : ");
        y = GetDouble("input y : ");

        // 入力された値を元に計算
        z = x * x + y * y; // z に x と y の二乗和を代入
        x /= z;            // x =  x / z; と同じ。
        y /= -z;           // y = -y / z; と同じ。

        // 計算結果を出力
        Console.Write("({0}, {1})", x, y);
    }

    /// <summary>
    /// 入力を促すメッセージを表示して、実数を入力してもらう。
    /// 正しく実数として解釈できる文字が入力されるまで繰り返す。
    /// <param name="message"> 入力を促すメッセージ </param>
    /// <return> 入力された値 </return>
    /// </summary>
    static double GetDouble(string message)
    {
        while (true)
        {
            // 入力を促すメッセージを表示して、値を入力してもらう
            Console.Write(message);
            double x;
            if (double.TryParse(Console.ReadLine(), out x))
                return x;

            // 不正な入力が行われた場合の処理
            Console.Write("error : 正しい値が入力されませんでした\n入力しなおしてください\n");
        }
    }

    /*
    // 旧バージョン(エラー処理がない)
    // 数値化できない文字列を入力すると、その時点でプログラムが止まる。

    static double GetDouble(string message)
    {
        Console.Write(message);
        double x = double.Parse(Console.ReadLine());
        return x;
    }
    */
}
