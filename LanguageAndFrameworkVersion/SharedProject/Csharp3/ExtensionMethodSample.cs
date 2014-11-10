#if Ver2Plus

using System.Linq;
using System.Text;

namespace VersionSample.Csharp3
{
    /// <summary>
    /// 拡張メソッドは、<see cref="System.Runtime.CompilerServices.ExtensionAttribute"/> が付いているだけの普通の静的メソッド。
    /// この属性は、同名であれば自作のものでもよくて、.NET 2.0 ターゲットでも、自作さえすれば拡張メソッドが使える。
    /// </summary>
    public static class ExtensionMethodSample
    {
        public static string SnakeToPascal(this string snake_case)
        {
            return string.Join("", snake_case.Split('_').Select(ToInitialUpper).ToArray());
        }

        public static string ToInitialUpper(this string s)
        {
            var sb = new StringBuilder();
            var first = true;
            foreach (var c in s)
            {
                if (first)
                {
                    sb.Append(char.ToUpper(c));
                    first = false;
                }
                else
                {
                    sb.Append(char.ToLower(c));
                }
            }
            return sb.ToString();
        }
    }
}

#endif
