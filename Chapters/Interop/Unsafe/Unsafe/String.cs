namespace Unsafe.String
{
    using static System.Console;

    unsafe class Program
    {
        static void Main()
        {
            var s = "abcde";
            fixed (char* p = s)
            {
                // 1行に1文字ずつ a b c d e が表示される
                for (int i = 0; i < s.Length; i++)
                    WriteLine(s[i]);
            }

            // ちなみに、string の場合は空文字列でも有効なアドレスが帰ってくる
            var empty = "";
            fixed (char* p = empty)
            {
                WriteLine((ulong)p);  // 非 0
                WriteLine((int)p[0]); // 常に '\0' が入ってる
            }
        }
    }
}
