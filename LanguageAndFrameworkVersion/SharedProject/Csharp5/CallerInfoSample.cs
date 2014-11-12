#if Ver2 && Plus

using System;
using System.Runtime.CompilerServices;

namespace VersionSample.Csharp5
{
    /// <summary>
    /// これも <see cref="Csharp3.ExtensionMethodSample"/> 同様、同名の属性クラス(自作でもいい)さえあれば動く。
    /// 自作すれば、.NET 2.0 でも動く。
    /// </summary>
    public class CallerInfoSample
    {
        public static void X()
        {
            WriteLine("sample");
        }

        public static void ApproxSameAsX()
        {
            WriteLine("sample", @"[your solution full path]\[project name]\Csharp5\CallerInfoSample.cs", 21, "ApproxSameAsX");
        }

        public static void WriteLine(string message,
            [CallerFilePath] string file = "",
            [CallerLineNumber] int line = 0,
            [CallerMemberName] string member = "")
        {
            var s = string.Format("{0}:{1} - {2}: {3}", file, line, member, message);
            Console.WriteLine(s);
        }
    }
}

#endif
