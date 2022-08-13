// (ref int, ref int) みたいな、「ref 戻り値の多値戻り値」を書きたい。

namespace RefField;

partial class Program
{
#if 本当に書きたかったもの
    private static (ref int, ref int) M(ref int a, ref int b, ref int c)
    {
        // 実際には何かしら分岐するとして、とりあえずデモ用に常に a, b を返す。
        return (ref a, ref b);
    }

    // ValueTuple<ref int, ref int> (要は ref 型引数)が書ければいいんだけども…
    // 要望としては出てるけども、ちょっと実装するとなると大変そう。
#endif

    // ref 多値戻り値代わりに RefTuple (後ろで実装)を使う。
    // これなら書ける。
    private static RefTuple<int, int> M(ref int a, ref int b, ref int c)
    {
        return new(ref a, ref b);
    }

    public static void MultipleRefReturnExample()
    {
        int x = 1;
        int y = 1;
        int z = 1;

        var (r1, r2) = M(ref x, ref y, ref z);

        r1.Reference = 2;
        r2.Reference = 3;

        Console.WriteLine((x, y, z)); // 2, 3, 1
    }
}

// 現状、(ref x, ref y) みたいなタプルを作れないので、その代わりになる型を用意。
ref struct RefTuple<T1, T2>
{
    public ref T1 Item1;
    public ref T2 Item2;

    public RefTuple(ref T1 item1, ref T2 item2)
    {
        Item1 = ref item1;
        Item2 = ref item2;
    }

    // ref のまま分解したいんだけど…
#if false
    // これは書けるけど、やりたいことじゃない。
    public void Deconstruct(out T1 item1, out T2 item2)
    {
        item1 = Item1;
        item2 = Item2;
    }

    // 本当にやりたいことはこれだけど、無理。
    // すなわち、ref tuple deconstruct 無理。
    public void Deconstruct(out ref T1 item1, out ref T2 item2)
    {
        item1 = ref Item1;
        item2 = ref Item2;
    }
#endif

    // ref deconstruct やろうとすると、
    // ByReference<T> みたいなものを介する必要あり。
    public void Deconstruct(out ByReference<T1> item1, out ByReference<T2> item2)
    {
        item1 = new(ref Item1);
        item2 = new(ref Item2);
    }
}

// せっかく ref field を書けるようになったけど、結局「ref field 1個だけ持つラッパー構造体」が必要に…
ref struct ByReference<T>
{
    public ref T Reference;
    public ByReference(ref T reference) => Reference = ref reference;
}
