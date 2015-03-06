using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Changed in VS 2010
///
/// In https://msdn.microsoft.com/en-us/library/vstudio/ee855831.aspx
/// A new implicit conversion is added for generic interfaces and delegates such as <see cref="IEnumerable{T}"/> and <see cref="Func{TResult}"/>.
/// </summary>
/// <remarks>
/// Type parameter T of <see cref="IEnumerable{T}"/> has been covariant since .NET 4 (VS 2010).
///
/// <code><![CDATA[
/// IEnumerable<string> s;
///
/// IEnumerable x = s;         // OK in all versions. IEnumerable<string> is inherited from IEnumerable.
/// IEnumerable<object> y = s; // OK in .NET 4 or later. string is inherited from object, so IEnumerable<string> can be treated like a subtype of IEnumerable<object> because of covariance.
/// ]]></code>
/// </remarks>
class ImplicitConversionCausedByVariance
{
    public static void Test(IEnumerable e)
    {
        Console.WriteLine("IEnumerable");
    }
    public static void Test(IEnumerable<object> e)
    {
        Console.WriteLine("IEnumerable<object>");
    }
    static void Main(string[] args)
    {
        Test(new List<string>());
        // Prints different results.
        // C# 2008: IEnumerable
        // C# 2010: IEnumerable<object>

        IEnumerable<string> strings =
            new List<string>();
        if (strings is IEnumerable<object>)
            Console.WriteLine("True");
        else
            Console.WriteLine("False");
        // Prints different results.
        // C# 2008: False
        // C# 2010: True
    }
}
