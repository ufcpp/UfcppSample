using System;

class Program
{
    static void Main()
    {
        Console.WriteLine(X(false)); // -1
        Console.WriteLine(X(true)); // 1

        unsafe
        {
            byte x = 2;
            bool y = *(bool*)&x;
            Console.WriteLine(X(y)); // C# 7.0 までは 0 だった。C# 8.0 で 1 になるように。
        }
    }

    static int X(bool b)
    {
        switch (b)
        {
            case false: return -1;
            case true: return 1;
            default: return 0;     // C# 7.0 までは何も言われなかった。C# 8.0 で「到達できないコード」警告出るように。
        }
    }
}