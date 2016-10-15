using System;

namespace ConsoleApplication1
{
    class CompatibleWithBstr
    {
        public static void WriteLayout()
        {
            WriteLayout("aαあ");
        }

        private unsafe static void WriteLayout(string s)
        {
            Console.WriteLine(s);
            Console.WriteLine(s.Length.ToString("X8"));

            fixed (char* ps = s)
            {
                byte* p = (byte*)ps;
                for (int i = -4; i < (s.Length + 1) * 2; i++)
                {
                    var b = *(p + i);
                    Console.Write(b.ToString("X2"));
                }
            }

            Console.WriteLine();
        }
    }
}
