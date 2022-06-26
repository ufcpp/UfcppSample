using NBitInt;
using NBitInt.IntConstants;

Int<byte, _1> a = default;
Int<byte, _2> b = default;
Int<byte, _4> c = default;
Int<byte, _7> d = default;
Int<short, _9> e = default;

for (int i = 0; i < 1024; i++, a++, b++, c++, d++, e++)
{
    Console.WriteLine($"{a,3} {b,3} {c, 3} {d, 3} {e, 3}");
}
