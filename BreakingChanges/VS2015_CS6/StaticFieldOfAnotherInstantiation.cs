public static class Foo<T>
{
    static Foo()
    {
        if (typeof(T) == typeof(int))
        {
            Foo<int>.compute = x => (int)x;
        }
    }

    public static readonly System.Func<double, T> compute;
}

/// <summary>
/// Can be compiled in: C# 3, 4, 5
///
/// https://github.com/dotnet/roslyn/issues/990
/// </summary>
class StaticFieldOfAnotherInstantiation
{
    static void Main()
    {
        System.Console.WriteLine(Foo<int>.compute(1));
    }
}
