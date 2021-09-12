using System.Runtime.CompilerServices;

m(2 * 3 * 5);

static void m(
    int x,
    [CallerLineNumber] int line = 0,
    [CallerFilePath] string? file = null,
    [CallerMemberName] string? member = null,
    [CallerArgumentExpression("x")] string? arg = null)
{
    Console.WriteLine($@"{file} の {line} 行目
{member} から呼ばれていて
{arg} という式を引数に渡している
(実際の値は {x})
");
}
