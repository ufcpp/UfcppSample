using System;

namespace ConsoleApp1.ExpressionVariables
{
    class FieldInitializer
    {
        // フィールド初期化子やプロパティ初期化子でも out var が使えるように。
        public int X = Table.TryGetValue("one", out var x) ? x : 0; // TryGetValue 成功 → 1
        public int Y { get; } = Table.TryGetValue("abc", out var y) ? y : 0; // TryGetValue 失敗 → 0

#if Uncompilable
        // フィールド/プロパティ初期化子中で宣言した変数は、その初期化子内でのみ有効。
        // なので↓これはエラーに。
        public int X2 = x;
        public int Y2 = y;
#endif

        // X の初期化子中の x は初期化子内でだけ有効なわけで、
        // 別の初期化子で改めて x を使っても平気。
        public int Z { get; set; } = Table.TryGetValue("two", out var x) ? x : 0; // TryGetValue 成功 → 2

        // ↑これ、「C# スクリプト動作」の時との一貫性はないんだけど。
        // スクリプト動作の時は、「変数宣言っぽく見えているものは実は見えないクラスのフィールドになってる」みたいな内部挙動なんだけど、
        // out var で宣言された変数も、フィールドになってて、その後もずっと有効な変数になる。
        //
        // とはいえ、そんな内部挙動とかユーザーからすると知ったことではないわけで。
        // 「X の初期化子で宣言した変数を Y の初期化子やコンストラクター内で使いたい」なんて思う人は少数派だろうし。
        // どう考えても便利さよりも事故るリスクの方が大きい。

        static void Main()
        {
            var x = new FieldInitializer();

            Console.WriteLine(x.X); // 1
            Console.WriteLine(x.Y); // 0
            Console.WriteLine(x.Z); // 2
        }
    }
}
