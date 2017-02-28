namespace ConsoleApp1._07_Future._71
{
    // C# 7.1 予定リスト
    // C# 7.0 の積み残し。大体仕様は固まっているけど、工数的な都合で実装されなかっただけ。

#if false

    using System;
    using System.Threading.Tasks;

    // Auto-Implemented Property Field-Targeted Attributes
    [Serializable]
    class SerializableSample
    {
        // 実はこいつのバッキングフィールドに NonSerialized 属性を付ける手段がない
        // ↓書けてるように見えて、コンパイル エラーにならないだけで実は無効(警告は出る)
        // 7.1 で付けれるように
        public int Value { get; set; }
    }

    // Expression variables in initializers
    class InitSample
    {
        public int Value { get; }

        public InitSample(int v) => Value = v;
        // フィールド初期化子、コンストラクター初期化子などで、今のところ out var など(宣言式)を使えない
        public InitSample(string s) : this(int.TryParse(s, out var x) ? x : 0) { }
    }

    // Mix Declarations and Variables in Deconstruction
    class DeconstructionSample
    {
        public static void Run()
        {
            var (x, y) = (1, 2); // OK。分解 + 宣言
            (x, y) = (y, x); // OK。分解 + 代入

            (var t, x) = (10, 20); // 今はダメ。宣言と代入の混在もできるべき
        }
    }

    // Discard for lambda parameters (_, _) =>
    class DiscardSample
    {
        public static void Run()
        {
            // OK。分解時の discard
            (_, var x) = (1, 2);

            // OK。out の discard
            if(int.TryParse("abc", out _))
                Console.WriteLine("parse 成功");

            // 今はダメ。ラムダ式の引数でも discard 使いたい
            EventHandler<string> e = (_, _) => Console.WriteLine("イベント");
        }
    }

    // Private protected
    class AccessibilitySample
    {
        // どこからでも参照可能
        public int Public;

        // 派生クラスから参照可能
        protected int Protected;

        // 同一アセンブリ内から参照可能
        internal int Internal;

        // このクラス内からだけ参照可能
        private int Pribate;

        // 派生クラスからも、同一アセンブリ内からも参照可能
        protected internal int ProtectedOrInternal;

        // 同一アセンブリ内にあって、かつ、派生クラスから参照可能
        // というアクセス レベルを作りたい
        private protected int ProtectedAndInternal;
    }

    // Improved common type
    class CommonTypeSample
    {
        private static Random _rand = new Random();
        private static bool Rand() => _rand.NextDouble() < 0.5;

        class Base { }
        class A : Base { }
        class B : Base { }

        public static void Run()
        {
            var r = new Random();

            // null を int? と認識してくれてもいいのに
            int? x = Rand() ? 1 : null;

            // ちなみに、これならOK
            int? x1 = Rand() ? (int?)1 : null;
            int? x2 = Rand() ? 1 : default(int?);

            // A, B の共通項取って、Base と認識してくれてもいいのに
            Base y = Rand() ? new A() : new B();

            // ちなみに、これならOK
            Base y1 = Rand() ? (Base)new A() : new B();
            Base y2 = Rand() ? new A() : (Base)new B();
        }
    }

    // "default" expression
    class DefaultExpressionSample
    {
        // 両辺に型名を書くのだるい
        int x0 = default(int);
        int? y0 = default(int?);
        string z0 = default(string);

        public static void Run()
        {
            // ローカル変数なら、左辺→右辺の型推論が効くけども
            var x = default(int);
            var y = default(int?);
            var z = default(string);
        }

        // 右辺からの型推論をしたいことがちらほら
        // 「default 式」を導入したい
        // 具体的な型は右辺から推論
        int x = default;
        int? y = default;
        string z = default;

        // 同様に、new も右辺から型推論する案あり
        object lockObj = new();
    }

    class AsyncMainSample
    {
        // ここをエントリーポイントにしたい
        // 今は void でないとエントリーポイントとして認識されない
        static async Task Main()
        {
            await Task.Delay(1);
        }
    }

#endif
}
