using static System.Console;

namespace X.Y
{
    class Program
    {
        static void Main()
        {
            // 単に Y って書くと、名前空間 X.Y の方の意味になる
            global::Y.F();
        }
    }
}

class Y { public static void F() => WriteLine("class B"); }
