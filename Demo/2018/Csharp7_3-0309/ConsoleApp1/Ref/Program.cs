using System;

namespace ConsoleApp1.Ref
{
    class Program
    {
        static void Main()
        {
            RefReassignment();
            RefFor();
            RefForEach();
        }

        /// <summary>
        /// ref ローカル変数に ref reassignment (参照先の再割り当て)ができるように。
        /// </summary>
        static void RefReassignment()
        {
            var x = 1;
            var y = 2;

            // r が x を参照中 → x が 10 に。
            ref var r = ref x;
            r = 10;

            // r の参照先を y に変更 → y が 20 に。
            r = ref y;
            r = 20;

            Console.WriteLine((x, y)); // (10, 20)
        }

        /// <summary>
        /// for ステートメントの初期化式で ref ローカル変数を宣言できるように。
        /// </summary>
        static void RefFor()
        {
            // スタック上に連結リストを構築。
            Span<Node> list = stackalloc[]
            {
                new Node(1, 3),
                new Node(3, 2),
                new Node(4, 4),
                new Node(2, 1),
                new Node(5, 0),
            };

            // ↑の Node を参照 for。
            for (ref var n = ref list[0]; ; n = ref list[n.NextIndex])
            {
                Console.WriteLine(n.Value);
                if (n.NextIndex == 0) break;
            }
        }

        /// <summary>
        /// foreach の反復変数も ref で宣言できるように。
        /// </summary>
        static void RefForEach()
        {
            var array = new[] { 1, 2, 3, 4, 5 };

            foreach (ref var r in new RefArrayEnumerable<int>(array))
            {
                // r は参照なので、これで配列の全要素を 99 に上書き。
                r = 99;

#if Uncompilable
                // ちなみに、foreach 変数は reassignment 不可
                r = ref array[0];
#endif
            }

            foreach (var x in array)
            {
                Console.WriteLine(x);
            }
        }
    }
}
