namespace Cs8InVs2019P1.NRT
{
// 有効にするには #nullable ディレクティブが必要。
#nullable enable

    using System;

    class Program
    {
        static void Main()
        {
            Console.WriteLine(LengthSum("abc", "xyz"));
            Console.WriteLine(LengthSum("abc", null));
        }

        static int LengthSum(string a, string? b)
        {
#if false
            // こう書いてしまうと b. のところで警告。
            var len0 = a.Length + b.Length;

            // これなら OK。b?. なので、b の null チェック済み。
            var len1 = a.Length + b?.Length ?? 0;
#endif

            // こんな感じで if で null チェックしても OK。
            // チェック済みな個所では b. で大丈夫。
            var len = a.Length;
            if(b != null) len += b.Length;

            return len;
        }
    }
}
