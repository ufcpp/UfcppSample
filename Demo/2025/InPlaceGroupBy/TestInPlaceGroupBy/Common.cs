namespace TestInPlaceGroupBy;

class Common
{
    public static (string key, int value)[] Data = [
        ("a", 1), ("ab", 2), ("abc", 3), ("a", 4), ("ab", 5),
        ("a", 6), ("a", 7), ("abc", 8), ("a", 9), ("a", 10),
        ("a", 11), ("ab", 12), ("abc", 13), ("b", 14), ("ab", 15),
        ("a", 16), ("a", 17), ("bc", 18), ("a", 19), ("b", 20),
        ("a", 21), ("ab", 22), ("ac", 23), ("a", 24), ("ab", 25),
        ("a", 26), ("d", 27), ("abc", 28), ("a", 29), ("ac", 30),
    ];

    public static int Compare((string key, int value) x, (string key, int value) y)
    {
        return x.key.AsSpan().CompareTo(y.key, StringComparison.Ordinal);
    }
}
