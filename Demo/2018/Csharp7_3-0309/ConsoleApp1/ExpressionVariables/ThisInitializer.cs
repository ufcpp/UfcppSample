using System;

namespace ConsoleApp1.ExpressionVariables
{
    class ThisInitializer
    {
        public ThisInitializer(int len) => Console.WriteLine(len);

        // コンストラクター初期化子内の式中で out var が使えるように。
        public ThisInitializer(string s)
            : this(Table.TryGetValue(s, out var x) ? x : 0)
        {
            // 初期化子内で宣言した変数は、コンストラクター内で有効。
            Console.WriteLine(x);
        }

        static void Main()
        {
            new ThisInitializer("one"); // TryGetValue 成功(戻り値 true、out 1)。 1, 1
            new ThisInitializer("abc"); // TryGetValue 失敗(戻り値 false、out -1)。 0, -1
        }
    }
}
