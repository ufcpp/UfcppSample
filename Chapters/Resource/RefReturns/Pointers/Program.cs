namespace RefReturns.Pointers
{
    class Program
    {
        public static ref int Max(ref int x, ref int y)
        {
            if (x >= y) return ref x;
            else return ref y;
        }

        public static unsafe int* Max(int* x, int* y)
        {
            if (*x >= *y) return x;
            else return y;
        }
    }
}
