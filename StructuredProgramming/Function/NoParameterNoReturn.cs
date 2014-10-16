using System;

/// <summary>
/// 引数がたくさんある関数、引数のない関数や、戻り値のない関数。
/// </summary>
class NoParameterNoReturn
{
    public static void Run()
    {
        int[] array = new int[3];

        // 乱数を使って値を生成
        for (int i = 0; i < array.Length; ++i)
        {
            array[i] = (int)(Random() >> 58); // [0,63] の整数乱数生成
        }

        // ノルムを計算
        double norm = Norm(array[0], array[1], array[2]);

        // 値の出力
        WriteArray(array);
        Console.Write("norm = {0}\n", norm);
    }

    static ulong seed = 4275646293455673547UL;

    /// <summary>
    /// 線形合同法による乱数の生成
    /// </summary>
    static ulong Random()
    {
        unchecked { seed = seed * 1566083941UL + 1; }
        return seed;
    }

    /// <summary>
    /// 入力した3つの値のノルムを計算
    /// <summary>
    static double Norm(double x, double y, double z)
    {
        return x * x + y * y + z * z;
    }

    /// <summary>
    /// 配列を , で各要素を区切って、{}で括った形式で出力
    /// <summary>
    static void WriteArray(int[] array)
    {
        Console.Write("{");
        for (int i = 0; i < array.Length - 1; ++i)
        {
            Console.Write("{0}, ", array[i]);
        }
        Console.Write(array[array.Length - 1] + "}\n");
    }
}
