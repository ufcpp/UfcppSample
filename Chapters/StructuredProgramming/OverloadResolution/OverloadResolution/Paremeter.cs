namespace OverloadResolution.Paremeter
{
    using System;

    // A → B → C の型階層
    // IDisposable インターフェイスを実装
    // C には int への暗黙的型変換あり
    class A : IDisposable { public void Dispose() { } }
    class B : A, IDisposable { }
    class C : B, IDisposable
    {
        public static implicit operator int(C x) => 0;
    }

    class Program
    {
        static void Main()
        {
            // M のオーバーロードがいくつかある中、C を引数にして呼び出す
            M(new C());
        }

        // 上から順に候補になる。
        // 上の方を消さないと、下の方が呼ばれることはない。

        // 「そのもの」が当然1番一致度高い
        static void M(C x) => Console.WriteLine("C");

        // 次がジェネリックなやつ。型変換が要らないので一致度が高いという扱い。
        static void M<T>(T x) => Console.WriteLine("generic");

        // 基底クラスは、階層が近い方が優先。この場合 B が先で、A が後
        static void M(B x) => Console.WriteLine("B");

        static void M(A x) => Console.WriteLine("A");

        // 次に、インターフェイス、暗黙的型変換が同率。
        // (構造体の時の ValueType と違って、クラスは明確に基底クラスが上。)
        // この2つが同時に候補になってると ambiguous エラー
        static void M(IDisposable x) => Console.WriteLine("IDisposable");
        static void M(int x) => Console.WriteLine("int");

        // 最後が object。
        static void M(object x) => Console.WriteLine("object");
    }
}
