namespace LocalFunctions.RecursiveFunction
{
    class Node
    {
        public int Value { get; }
        public Node? Left { get; }
        public Node? Right { get; }

        public Node(int value, Node? left = null, Node? right = null)
        {
            Value = value;
            Left = left;
            Right = right;
        }
    }
}
