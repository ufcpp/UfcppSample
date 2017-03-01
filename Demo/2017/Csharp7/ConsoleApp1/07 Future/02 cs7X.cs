namespace ConsoleApp1._07_Future._7X
{
    // C# 7.X 予定リスト
    // 7.1, 7.2 よりは具体性がなく、7.3 になるか 7.4 になるか… みたいなあいまいな感じ。

#if false

    using System;
    using System.Collections.Generic;
    using ConsoleApp1._02_Patterns;
    using System.Linq;
    using System.Threading.Tasks;
    using global::System;

    // Support for == and != on tuple types
    class TupleEqualsSample
    {
        public static void Run()
        {
            // プリミティブ同士の == は型変換が掛かったり、
            int i = 1;
            double d = 1.0;
            Console.WriteLine(i == d); // true
            Console.WriteLine(i.Equals(d)); // 型があっていないので false

            // NaN 同士の比較の挙動が Equals と違ったり
            var n1 = double.NaN;
            var n2 = double.NaN;
            Console.WriteLine(n1 == n2); // false
            Console.WriteLine(n1.Equals(n2)); // true

            // Tuple には Equals はある。メンバーごとの Equals はある
            Console.WriteLine((1, 1).Equals((i, d))); // 型があっていないので false
            Console.WriteLine((double.NaN, double.NaN).Equals((n1, n2))); // true

            // Tuple にも == があってもいいのではないか
            Console.WriteLine((1, 1) == (i, d)); // 1 == i && 1 == d を期待 → true
            Console.WriteLine((double.NaN, double.NaN) == (n1, n2)); // double.NaN == n1 && double.NaN == n2 を期待 → false
        }
    }

    class DeconstructionInQuerySample
    {
        public static void Run()
        {
            // 変数宣言で分解
            var (x, y) = (1, 2);

            // foreach で分解
            var items = new[] { 2, 3, 5, 7 }.Select((item, index) => (item, index));
            foreach (var (item, index) in items)
            {
            }

            // なら、クエリ式中でも分解構文を使えるべきではないか
            var q =
                from (item, index) in items               // from で分解
                let (a, b) = (item + index, item * index) // let で分解
                select a * b;

            // ラムダ式の引数でも分解できるべきではないか
            var ma = items.Select(var (a, b) => a * b).Sum();
        }
    }

    // params IEnumerable<T>, ICollection<T> and IList<T>
    class ParamesEnumerableSample
    {
        // これまで、params (可変長引数)にしたければ、引数の型は配列になる
        public static int Sum1(params int[] items) => items.Sum();

        // IEnumerable など、インターフェイスにしたければ、一段ラップが必要だった
        public static int Sum2(params int[] items) => Sum2(items.AsEnumerable());
        public static int Sum2(IEnumerable<int> items) => items.Sum();

        // 直接、params IEnumerable<T> とかを書かせてほしい
        public static int Sum3(params IEnumerable<int> items) => items.Sum();
        public static int Sum3(params List<int> items) => items.Sum();

        public static void Run()
        {
            Sum3(1, 2, 3); // どう展開されるべき？配列？
            Sum3(new List<int> { 1, 2, 3 }); // 配列でないものに展開されるとして、コレクション初期化子(Add メソッド)を呼ぶべき？
            Sum3(new List<int>(new[] { 1, 2, 3 })); // コンストラクターを呼ぶべき？
        }
    }

    // Target-typed `new` expression
    class TargetTypedNewSample
    {
        // ちょっとさすがにこんな長い型名を左右両辺に書きたくないんだけど…
        static Dictionary<(int a, int b), ((string first, string last) name, int id)> _table1 = new Dictionary<(int a, int b), ((string first, string last) name, int id)>();

        public static void LocalVar()
        {
            // ローカル変数なら、左辺→右辺の型推論が効くけども
            var table = new Dictionary<(int a, int b), ((string first, string last) name, int id)>();
        }

        // フィールドとかに var を認めてしまうと、型推論に掛かる時間が非線形になったり、ちょっとしたことで public API が変わっちゃったり、まずい
        // じゃあ、逆に、左辺から右辺の型を推論したい
        static Dictionary<(int a, int b), ((string first, string last) name, int id)> _table2 = new();

        // 戻り値とか
        static Dictionary<(int a, int b), ((string first, string last) name, int id)> Return() => new();

        // 引数とかでも同様
        static void Param(Dictionary<(int a, int b), ((string first, string last) name, int id)> table) { }
        static void Caller() => Param(new());
    }

    // Pattern Matching
    static class NodeExtensions
    {
        // 02 Patterns で例に出したやつをちょこっと変更
        public static Node Reduce(this Node n)
        {
            // if-else の連続よりも、case-when 並べる方がきれいなことが
            switch (n)
            {
                case Add(Const x, Const y): return new Const(x.Value + y.Value);
                case Add(Const x, var y) when x.Value == 0: return y;
                case Add(var x, Const y) when y.Value == 0: return x;
                case Mul(Const x, Const y): return new Const(x.Value * y.Value);
                case Mul(Const x, var y) when x.Value == 0: return new Const(0);
                case Mul(var x, Const y) when y.Value == 0: return new Const(0);
                case Mul(Const x, var y) when x.Value == 1: return y;
                case Mul(var x, Const y) when y.Value == 1: return x;
                default: return n;
            }
        }
    }

    // Null-conditional await
    class NullAwaitSample
    {
        // 既定動作は何もせず、派生クラスの一部でだけ何か処理を入れたいとき
        public virtual Task<int> M1() => null; // これだと、await M1(); でぬるぽ
        public virtual Task<int> M2() => Task.FromResult(0); // これだと、呼び出すたびに Task のアロケーションが発生

        private Task<int> _default = Task.FromResult(0);
        public virtual Task<int> M3() => _default; // 書くのだるい

        public async Task Run()
        {
            // await にも null 条件版が必要なのではないか
            int? n1 = await? M1();

            // ↓こう展開されることを期待
            var t = M1();
            int? n2 = t == null ? default(int?) : await t;
        }
    }

    // null coalescing assignment
    class NullAssignmentSample
    {
        private static int CalculateValue()
        {
            // 計算に結構時間が掛かるものとして
            return 0;
        }

        // キャッシュとかでよくあるパターン
        // 長い…
        private static int? _cache;

        public static int CachedValue
        {
            get
            {
                if (_cache == null)
                {
                    _cache = CalculateValue();
                }
                return _cache.Value;
            }
        }

        // 多少縮める手段はあるけども、これはこれでキモイ構文
        // 式の途中で代入(副作用のある式)を書いてるし、最後の .Value アクセスが冗長だし
        public static int CachedValue1 => _cache ?? (_cache = CalculateValue()).Value;

        // 専用の構文があってもいいのではないか
        public static int CachedValue2 => _cache ??= CalculateValue();
    }


    // Bestest Betterness
    // オーバーロード解決の際、「より一致度の高い方を選ぶ」というのを指して「betterness rule」ってのを決めてる
    // 「オーバーロード解決が賢くなったよ」ってのを指して「better betterness」って言ってたのが、
    // その次のバージョンで best betterness
    // さらに今、bestest betterness
    // とか言ってる。

    interface IA { }
    interface IB { }
    class X : IA, IB { }

    class StaticResolutionSample
    {
        // 呼び分けが難しそうなもの
        static void F(IA a) { }
        void F(IB b) { }

        // 比較用
        static void FA(IA a) { }
        void FB(IB b) { }

        void X()
        {
            Action<X> fa = FA; // OK
            Action<X> fb = FB; // OK

            // 現状、解決不能
            Action<X> a = F;      // this が付いてなかったら static 扱いで IA の方でいいのではないか
            Action<X> b = this.F; // this を付けたら IB の方
        }
    }

    class ConstraintResolutionSample
    {
        // 呼び分けが難しそうなもの
        static void F<T>(T x) where T : struct { }
        static void F<T>(IEnumerable<T> x) { }

        // 比較用
        static void FT<T>(T x) where T : struct { }
        static void FE<T>(IEnumerable<T> x) { }

        void X()
        {
            int[] array = null;

            FT(array); // NG。struct 制約に合わない
            FE(array); // OK

            // 現状、解決不能
            F(array); // F(T) の方は制約に合わないから候補から外して、F(IEnumerable<T>) の方を選べるべきではないか
        }
    }

    class ReturnResolutionSample
    {
        // 呼び分けが難しそうなもの
        static int F(IA x) => 0;
        static string F(IB x) => "";

        // 比較用
        static int FI(IA x) => 0;
        static string FS(IB x) => "";

        void X()
        {
            Func<X, int> fi = FI;    // OK。引数に関して反変性が効いてる
            Func<X, string> fs = FS; // OK。同上

            // 現状、解決不能
            Func<X, int> f1 = F;    // 戻り値の型も見て、int F(IA) を選べるべきではないか
            Func<X, string> f2 = F; // 同上、string F(IB)
        }
    }

    // Compiler intrinsics
    namespace System.Runtime.CompilerServices
    {
        [AttributeUsage(AttributeTargets.Method)]
        internal class CompilerIntrinsicAttribute : Attribute { }
    }

    class CompilerIntrinsicsSample
    {
        // まれに、C# では書けない IL 命令を使いたいことがある
        // 例: https://github.com/dotnet/corefx/tree/master/src/System.Runtime.CompilerServices.Unsafe/src
        // やろうと思えば、そこだけ IL アセンブラーを使えばいいんでできるけど、ビルド プロセスが面倒になったり

        // なので C# 中に IL 命令を埋め込める「インライン アセンブラー」機能が欲しいという話もなくはない
        // これはこれで、「2種類の言語を保守する羽目になる」「ildasm との2重管理になる」などの理由があってやりたくない

        // そこで、特定のメソッド呼び出しを特定の IL 命令に置き換えるような特殊対応で考える
        // 使ってる側からするとただのメソッド呼び出しだし、既存の C# パーサーで解釈できるし
        // 一番低コストで当初目的を果たせそう

        [System.Runtime.CompilerServices.CompilerIntrinsic]
        static unsafe extern void* LoadFunctionPointer(Delegate target);  // ldftn 命令に置き換え
    }

#endif
}
