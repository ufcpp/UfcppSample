using System;

class ArrayInitialization
{
    public static void Run()
    {
        const int N = 1024 * 1024;
        var points = new Vector4[N];

        // 中身が全部 0 なことを確認してみる
        unsafe
        {
            fixed (Vector4* pp = points)
            {
                // 無理やり byte 配列扱いして、1 byte ずつ確認
                var p = (byte*)pp;

                for (int i = 0; i < N * sizeof(Vector4); i++)
                {
                    if (p[i] != 0)
                        Console.WriteLine("絶対通らないはず");
                }
            }
        }
    }
}

struct Vector4
{
    public float x;
    public float y;
    public float z;
    public float w;

    /*
    // C# 6.0 では、引数なしのコンストラクターが定義できるようになるけども、これは配列初期化時には呼ばれない。
    // 配列初期化時には、コンストラクターで初期化するんじゃなくて機械的に 0 埋め
    public Vector4()
    {
        Console.WriteLine("配列初期化では呼ばれない");
    }
    */
}
