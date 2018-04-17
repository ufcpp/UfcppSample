namespace OverloadResolution.Csharp73.MethodReturn
{
    using System;

    class Program
    {
        static void M(Func<int> f) => Console.WriteLine("int");
        static void M(Func<string> f) => Console.WriteLine("string");

        static int IntReturn() => 0;
        static string StringReturn() => "";

        static void Main()
        {
            // ラムダ式賢い。
            M(() => 0); // int の方
            M(() => "abc"); // string の方

            // こういう書き方なら C# 7.2 まででもできた。
            M(() => IntReturn());
            M(() => StringReturn());

            // なのに、以下のような書き方はこれまでできなかった。
            // C# 7.3 からできるように。
            M(IntReturn);
            M(StringReturn);
        }
    }
}
