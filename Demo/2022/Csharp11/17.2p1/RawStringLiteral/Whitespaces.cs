// see https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/lexical-structure

namespace RawStringLiteral;

internal class Whitespaces
{
    // C# is not whitespace sensitive.
	               public const string S = """
	               abc
	               """;

    public static void M()
    {
        // Whitespace
        //     : [\p{ Zs}]  // any character with Unicode class Zs
        //     | '\u0009'  // horizontal tab
        //     | '\u000B'  // vertical tab
        //     | '\u000C'  // form feed
        //     ;
        foreach (var c in "	                ")
        {
            Console.WriteLine($"{(int)c:X}");
        }
    }
}
