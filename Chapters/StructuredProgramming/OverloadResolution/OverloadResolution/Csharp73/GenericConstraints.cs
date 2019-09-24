namespace OverloadResolution.Csharp73.GenericConstraints
{
    using System;

    // オーバーロード用のダミー型
    struct A { }
    struct B { }

    // IDisposable, IComparable な型を用意
    struct Disposable : IDisposable { public void Dispose() { } }
    struct Comparable : IComparable { public int CompareTo(object? x) => 0; }

    class Program
    {
        // M(x) で呼べるメソッドが2つ。
        // 差は、T の型制約のみ。
        static void M<T>(T x, A _ = default) where T : IDisposable { }
        static void M<T>(T x, B _ = default) where T : IComparable { }

        static void Main()
        {
            // C# 7.3 からこの呼び出し方ができるように。
            M(new Disposable());
            M(new Comparable());

            // この書き方も C# 7.3 から。
            M(new Disposable(), default); // default は default(A) に推論される
            M(new Comparable(), default); // default は default(B) に推論される

            // C# 7.2 以前の場合、こう書くのが必須。
            M(new Disposable(), default(A));
            M(new Comparable(), default(B));
        }
    }
}
