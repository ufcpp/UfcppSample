using System.Collections.Generic;

/// <summary>
/// Changed in VS 2008 SP1
///
/// #1 in https://msdn.microsoft.com/en-us/library/cc713578.aspx
/// Type inference is now included on arrays of pointer types in method overload resolution.
/// </summary>
unsafe class OverloadResolutionArraysOfPointer
{
    static void Main()
    {
        IEnumerable<int*[]> y = null;
        Test(y);
    }

    // Selected by Visual C# 2008 SP1.
    static void Test<S>(IEnumerable<S> x) { System.Console.WriteLine("Test<S>(IEnumerable<S> x)"); }

    // Selected by Visual C# 2005.
    static void Test(object o) { System.Console.WriteLine("Test(object o)"); }
}
