#define X

global using System.Text;
using System.Buffers;

// aaa

var x = int.Parse(Console.ReadLine()!);
Console.WriteLine($"{x:X,8}");
Console.WriteLine(@$"--- {x} ---");
Console.WriteLine($@"--- {x} ---");
Console.WriteLine(@"\w+\d.\.txt");

foreach (var i in new[] { 1, 0, 2, -1, 3 })
{
    if ((i & 1) == 0) { }
}

_ = 1;

/// <summary>
/// <see cref="int.Parse(ReadOnlySpan{char}, System.Globalization.NumberStyles, IFormatProvider?)"/>
/// </summary>
void m()
{
}

#if X
void m1() { }
#endif

#if Y
void m2() { }
#endif
