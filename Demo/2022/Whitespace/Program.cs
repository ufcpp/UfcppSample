//↓ U+0001～001F except for tab (09), CR (0D), LF (0A)
//
// Visual Studio displays these control characters as symbols.

// U+000B, U+000C, and U+001A are treated as whitespace in C#, thus, the folloing is a valid C# code.
var a = 1;
if (a > 0)
Console.WriteLine(a);
