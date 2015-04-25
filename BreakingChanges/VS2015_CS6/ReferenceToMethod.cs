using System;

/// <summary>
/// Can be compiled in: C# 3, 4, 5
///
/// https://github.com/dotnet/roslyn/issues/2249
///
/// The native C# compiler (C# 3, 4, 5) accepts the following code due to a bug.
/// </summary>
class ReferenceToMethod
{
    static void M() { }
    public static void Main(string[] args)
    {
        var a = new Action(ref M);
    }
}
