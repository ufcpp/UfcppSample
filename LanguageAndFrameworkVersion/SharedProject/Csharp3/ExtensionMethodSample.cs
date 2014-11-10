#if Ver2Plus

using System.Linq;
using System.Text;

namespace VersionSample.Csharp3
{
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
