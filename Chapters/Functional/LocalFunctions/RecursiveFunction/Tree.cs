namespace LocalFunctions.RecursiveFunction
{
    using System.Collections.Generic;

    class Tree
    {
        // サンプル用に値を固定して作成
        private Node _root =
            new Node(1,
                new Node(2,
                    new Node(3),
                    new Node(4)
                    ),
                new Node(5,
                    new Node(6),
                    new Node(7)
                    )
            );

        public IEnumerable<int> Inorder()
        {
            return Inorder(_root); // フィールドを渡して再帰呼び出しを開始
        }

        // 再帰的にノードをたどる
        private static IEnumerable<int> Inorder(Node n)
        {
            if (n.Left != null)
                foreach (var x in Inorder(n.Left))
                    yield return x;

            yield return n.Value;

            if (n.Right != null)
                foreach (var x in Inorder(n.Right))
                    yield return x;
        }
    }
}
