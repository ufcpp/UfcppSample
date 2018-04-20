namespace Unsafe.MutateString
{
    unsafe class Program
    {
        static void Main()
        {
            var s = "abcde";

            fixed(char* p = s)
            {
                for (int i = 0; i < 5; i++)
                    p[i] = (char)(i + '1');
            }

            System.Console.WriteLine(s); // 12345
        }
    }
}
