namespace TestPseudoDictionary;

internal class Common
{
    public static bool EqualsIgnoreCase(string x, string y) => x.Equals(y, StringComparison.OrdinalIgnoreCase);

    public struct X
    {
        public string Key;
        public int Value;
    }

    public static string GetKey(X x) => x.Key;
    public static X NewX(string key) => new() { Key = key };

    public const string Data = "a ab Abc x A a xy x abc ab A X y z xy b abc a X yZ zX z a b c x ab aBc";
    public static IEnumerable<string> EnumerateWords() => Data.Split(' ');
}
