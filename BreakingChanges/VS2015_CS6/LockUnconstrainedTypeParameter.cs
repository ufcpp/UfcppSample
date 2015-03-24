class SOptions { }
class Driver
{
    public void DoSomething<T>(T data)
    {
        lock (data) // << Generates CS0185
        { }
    }
    public void DoSomethingElse(SOptions data)
    {
        lock (data) // << Works fine
        { }
    }
    public void DoSomething<T>(T data)
        where T : class
    {
        lock (data) // Works fine
        { }
    }
}

/// <summary>
/// Can be compiled in: C# 2, 3, 4, 5
/// but, it is unsafe - incorrectly locked - if the T is a value type.
///
/// https://github.com/dotnet/roslyn/issues/1067
/// CS0185 (C#) experienced on 'lock' statement in unconstrained type parameter
/// </summary>
/// <remarks>
/// FYI, it is legal if the T is constrained as class 
/// <code><![CDATA[
/// public void DoSomething<T>(T data)
///     where T : class
/// {
///     lock (data) { }
/// }
/// ]]></code>
/// </remarks>
class LockUnconstrainedTypeParameter
{
    static void Main(string[] args)
    {
        SOptions SOpt = new SOptions();
        Driver D = new Driver();
        D.DoSomething<SOptions>(SOpt);
        D.DoSomethingElse(SOpt);
    }
}
