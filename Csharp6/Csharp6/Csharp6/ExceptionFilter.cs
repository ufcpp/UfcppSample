namespace Csharp6.Csharp6.ExceptionFilter
{
    using System;

    class Program
    {
        static void Main()
        {
            try
            {
                SomeMethod(1, 2);
            }
            catch (ArgumentException e) when (e.ParamName == "x")
            {
                // パラメーター名が x の時だけはエラー無視
            }
        }

        private static void SomeMethod(int x, int y)
        {
            if (x < 0) throw new ArgumentException(nameof(x));
            if (y < 0) throw new ArgumentException(nameof(y));
        }
    }
}
