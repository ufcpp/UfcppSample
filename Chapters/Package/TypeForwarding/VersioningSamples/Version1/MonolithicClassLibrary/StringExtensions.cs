public static class StringExtensions
{
    public static string ToLower(this string s) =>
        s == null ? null :
        char.ToLower(s[0]) + s.Substring(1, s.Length - 1);
}
