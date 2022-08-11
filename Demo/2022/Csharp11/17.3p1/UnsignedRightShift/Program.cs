using System.Numerics;

//## >>> で論理シフト

// signed な整数に対して、
int x = -1;

// >>> で右シフトすると論理シフト(符号ビットを引き継がない右シフト)になる。
Console.WriteLine(x >>> 16); // 65535

// >> だと算術シフトで、 -1 はいくら右シフトしても -1。
Console.WriteLine(x >> 16);

// これまでの C# だと、「論理シフトしたければ一度 uint にキャストしてくれ」ということになってた。
Console.WriteLine((uint)x >> 16); // 65535

//## generic math
// generic math が入ると、「T に対応する unsigned な型を取る手段がない」ということになって、 >>> が必要になったみたい。

// unsigned の場合は想定通り動くけど、
Console.WriteLine(m<ushort>(0xffff)); // 0x0ff0

// signed のときダメ。
Console.WriteLine(m<short>(-1)); // 0xfff0

static T m<T>(T x)
    where T : IShiftOperators<T, int, T>
{
    // 適当に「両端4ビットずつの値を消す」みたいなことをシフトでやるとして、
    x <<= 8;
    x >>= 4; // signed int の場合、算術シフトになっちゃう！
    return x;
}
