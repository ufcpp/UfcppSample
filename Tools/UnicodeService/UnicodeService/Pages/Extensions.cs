using System.Collections.Generic;
using System.Text;

namespace UnicodeService.Pages
{
    public static class Extensions
    {
        public static IEnumerable<(bool first, T item)> IsFirst<T>(this IEnumerable<T> source)
        {
            bool first = true;
            foreach (var item in source)
            {
                yield return (first, item);
                first = false;
            }
        }

        public static byte[] GetBytes(this string s) => Encoding.UTF8.GetBytes(s);

        public static string Hex(this byte x) => x.ToString("X");
        public static string Hex(this uint x) => x.ToString("X");
    }
}
