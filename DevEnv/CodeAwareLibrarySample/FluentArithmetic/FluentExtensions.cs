namespace FluentArithmetic
{
    public static class FluentExtensions
    {
        public static int Add(this int x, int y) => x + y;
        public static int Sub(this int x, int y) => x - y;
        public static int Mul(this int x, int y) => x * y;
        public static int Div(this int x, int y) => x / y;
    }
}
