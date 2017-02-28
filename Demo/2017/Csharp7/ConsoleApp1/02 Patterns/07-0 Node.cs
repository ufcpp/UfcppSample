namespace ConsoleApp1._02_Patterns
{
    abstract partial class Node { }

    partial class Var : Node { }

    partial class Const : Node
    {
        public int Value { get; }
        public Const(int value) { Value = value; }
    }

    partial class Add : Node
    {
        public Node X { get; set; }
        public Node Y { get; set; }
        public Add(Node x, Node y) { X = x; Y = y; }
    }

    partial class Mul : Node
    {
        public Node X { get; set; }
        public Node Y { get; set; }
        public Mul(Node x, Node y) { X = x; Y = y; }
    }
}
