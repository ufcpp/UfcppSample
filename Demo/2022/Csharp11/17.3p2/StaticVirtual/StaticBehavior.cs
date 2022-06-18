// 静的なのとインスタンスなのと、1個ずつメンバーを用意。
interface IName
{
    static abstract string StaticName { get; }
    string InstanceName { get; }
}

// IName を実装するクラスを派生させて作る。
class Base : IName
{
    static string IName.StaticName => "static Base";
    string IName.InstanceName => "instance Base";
}

class Derived : Base, IName
{
    static string IName.StaticName => "static Derived";
    string IName.InstanceName => "instance Derived";
}

// 呼び出し側:
class StaticBehavior
{
    public static void WriteName<T>(T x)
        where T : IName
    {
        Console.WriteLine(x.InstanceName); // 通常の abstract
        Console.WriteLine(T.StaticName);   // static abstract
    }

    public static void M()
    {
        // この2つは型引数の静的な型とインスタンスの型が一致しているので変なことにはならない。
        WriteName(new Base());
        WriteName(new Derived());

        // こういうのがちょっと注意が必要。
        // 型引数は Base だけど、 x に渡る引数には Derived が入ってる。
        // 出力は、
        // instance Derived
        // static Base
        // になる。
        WriteName<Base>(new Derived());

        // コンパイル時に渡した型引数によって動作が決まっちゃう。
        // 動的な挙動は全然しない。
    }
}
