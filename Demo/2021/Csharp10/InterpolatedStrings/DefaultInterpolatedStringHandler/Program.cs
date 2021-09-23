Console.WriteLine(m(1, 2));
Console.WriteLine(m1(1, 2));
Console.WriteLine(await m2(Task.Delay(1).ContinueWith(_ => 1)));

string m(int a, int b) => $"{a} / {b}";

// m の展開結果
string m1(int a, int b)
{
    System.Runtime.CompilerServices.DefaultInterpolatedStringHandler h = new(3, 2);
    h.AppendFormatted(a);
    h.AppendLiteral(" / ");
    h.AppendFormatted(b);
    return h.ToStringAndClear();
}

// DefaultInterpolatedStringHandler に展開できない例 (await と併用が無理)
async Task<string> m2(Task<int> a) => $"result: {await a}";
