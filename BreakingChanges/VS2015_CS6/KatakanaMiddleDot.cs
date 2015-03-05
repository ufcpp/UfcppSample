using System;

/// <summary>
/// Can be compiled in: C# 2, 3, 4, 5
/// 
/// Breaking Change in C# 6.0, Roslyn new compiler.
/// Katakana Middle Dot "・" (カタカナ中点、中黒、U+30FB) no longer available for identifiers in C# 6.0.
/// </summary>
/// <remarks>
/// Katakana Middle Dot is a Japanese character whose usage is similar to hyphen in English.
/// Its Unicode class was Pc (Punctuation, Connector) in Unicode 5.1 or older, but it becomes Po (Punctuation, Other).
/// Pc characters can be used for C# identifiers, but Po cannot.
/// Looks like Roslyn adopts Unicode 6.0 or newer, so Katakana Middle Dot cannot be used for identifiers in C# 6.0. Java 7 has faced the same problem.
/// </remarks>
class KatakanaMiddleDot
{
    static void Main(string[] args)
    {
        int x・y = 10;
        Console.WriteLine(x・y);
    }
}