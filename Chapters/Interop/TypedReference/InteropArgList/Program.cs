namespace InteropArglist
{
    using System.Runtime.InteropServices;

    class Program
    {
        static void Main(string[] args)
        {
            printf("%d, %s, %c, %f", __arglist(1, "aaa", 'x', 1.5));
        }

        [DllImport("msvcrt", CallingConvention = CallingConvention.Cdecl)]
        static extern int printf(string format, __arglist);
    }
}
