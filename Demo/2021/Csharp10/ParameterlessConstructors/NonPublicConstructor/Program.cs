namespace ParameterlessConstructors.NonPublicConstructor;

#if false

// 引数なしコンストラクターは public でないとダメ。
// Activator.CreateInstance(typeof(A)) が new A() を返すのか default(A) を返すのかわからなくなるから避けたらしい。
struct A
{
    public int X;
    private A() => X = 0; // エラー
}

struct B
{
    public int X;
    internal B() => X = 0; // エラー
}

#endif
