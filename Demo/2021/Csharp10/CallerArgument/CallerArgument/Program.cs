using System.Runtime.CompilerServices;

m(2 * 3 * 5);

static void m(
    int x,
    [CallerArgumentExpression("x")] string? expression = null)
{
    Console.WriteLine($"{expression} = {x}");
}
