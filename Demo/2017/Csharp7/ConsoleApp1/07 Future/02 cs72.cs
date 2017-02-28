namespace ConsoleApp1._07_Future._72
{
    // C# 7.2 予定リスト
    // Span (System.Memory) とか Utf8String (System.Text.Primitives) とかに合わせて
    // interop シナリオ、生バイナリ操作など、パフォーマンスがらみの機能強化

#if false

    using System;
    using System.Text.Utf8;

    // blittable
    struct Point
    {
        // こういう、中身がプリミティブだけで構成されてる型は、本来ポインターにできたり、unsafe 的に融通が利く
        // (C# 仕様書上) Unmanaged 型って呼ばれてる
        // P/Invoke の文脈的に「blittable」って呼ばれてる。データのブロック転送可能、マーシャリングが低コストな型という意味
        public int X;
        public int Y;
    }

    unsafe class BlittableSample
    {
        private Point _point;

        // この通り、非ジェネリックならポインター化可能
        public Point* Alloc() => (Point*)Interop.malloc(sizeof(Point));

        // ジェネリックだと、今は無理
        // blittable 制約が欲しい
        public T* Alloc<T>()
            where T : blittable
            => (T*)Interop.malloc(sizeof(Point));
    }

    // readonly for locals and parameters
    class ReadonlySample
    {
        public static void Run(readonly int x)
        {
            x = 1; // 引数の書き換えとかバグの原因だし禁止したい

            let y = x;

            y = 10; // ローカル変数も、意図せず書き換えれないようにしたい

            // 変数宣言に readonly を付けるか
            // let を使うか
        }
    }

    // ref extension methods on structs
    static class RefExtensionMethodSample
    {
        // コピーコストを避けたいって意味だと、拡張メソッドでも構造体を ref 渡ししたい
        static ref int Min(this ref Point p)
        {
            ref int x = ref p.X;
            ref int y = ref p.Y;
            if (x < y) return ref x;
            else return ref y;
        }
    }

    // slicing
    class SlicingSample
    {
        public static void Run()
        {
            // 近々、System.Memory パッケージってのがリリースされる
            // 配列の一部分を参照する Span っていうクラスが追加される
            var array = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var slice1 = new Span<int>(array).Slice(2, 3); // 2, 3, 4 の部分を「参照」

            // これに対して、C# 構文的にも何かサポートが欲しい
            var slice2 = array[2:5];
        }
    }

    // utf8 string literals
    class Utf8StringLiteralSample
    {
        public static void Run()
        {
            // 近々、System.Text.Primitives パッケージってのがリリースされる
            // UTF8 エンコードされた byte 列をそのまま文字列操作できるクラスが追加される

            // これだと、内部的に UTF16 → UTF8 変換が掛かって無駄
            var s1 = new Utf8String("あ亜🐕");

            // これだと、読めない。つらい
            var s2 = new Utf8String(new byte[] { 227, 129, 130, 228, 186, 156, 240, 159, 144, 149 });

            // UTF8 用の、何らかのリテラル構文が欲しい
            Utf8String s3 = "あ亜🐕";
        }
    }

    // pattern-based `with` expressions
    class ImmutablePoint
    {
        // immutable なクラスを用意
        public int X { get; }
        public int Y { get; }
        public ImmutablePoint(int x, int y) => (X, Y) = (x, y);

        // こんな感じの、現在の値をコピーしてくれるような「規定値」が欲しい
        public ImmutablePoint With(int X = this.X, int Y = this.Y) => new ImmutablePoint(X, Y);
    }

    class WithExpressionSample
    {
        public static void Run()
        {
            var a = new ImmutablePoint(1, 2);

            // Y だけ書き換えたいとき、結構煩雑
            var b = new ImmutablePoint(a.X, 3);

            // こんな感じで略記したい
            var c = a with { Y = 3 };

            // ↑これは、↓こういう意味に
            var d = a.With(Y: 3);
        }
    }

    // Readonly ref
    class ReadonlyRefSample
    {
        // メソッド内部で書き換えたいから ref なのか、
        // コピーコストを避けるためだけに ref なのかが不明
        static ref int Min(ref Point p)
        {
            ref int x = ref p.X;
            ref int y = ref p.Y;
            if (x < y) return ref x;
            else return ref y;
        }

        // 書き換えないことを明示したい
        // ref の前後に readonly を付ける案とか
        // 引数に「in」修飾子を付ける案とか
        static ref readonly int Min(in Point p)
        {
            ref readonly int x = ref p.X;
            ref readonly int y = ref p.Y;
            if (x < y) return ref x;
            else return ref y;
        }
    }

#endif
}
