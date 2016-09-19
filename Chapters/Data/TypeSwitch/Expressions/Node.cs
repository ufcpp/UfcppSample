namespace TypeSwitch.Expressions
{
    public partial class Node { }

    public partial class Var : Node { }

    public partial class Const : Node
    {
        public int Value { get; }
        public Const(int value) { Value = value; }
    }

    public partial class Add : Node
    {
        public Node Left { get; }
        public Node Right { get; }
        public Add(Node left, Node right)
        {
            Left = left;
            Right = right;
        }
    }

    public partial class Mul : Node
    {
        public Node Left { get; }
        public Node Right { get; }
        public Mul(Node left, Node right)
        {
            Left = left;
            Right = right;
        }
    }
}
