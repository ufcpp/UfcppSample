namespace Patterns.Expressions
{
    public abstract class Node
    {
        public static readonly Node X = new Var();
        public static implicit operator Node(int value) => new Const(value);
        public static Node operator +(Node left, Node right) => new Add(left, right);
        public static Node operator *(Node left, Node right) => new Mul(left, right);
    }

    public class Var : Node { public override string ToString() => "x"; }

    public class Const : Node
    {
        public int Value { get; }
        public Const(int value) { Value = value; }
        public void Deconstruct(out int value) => value = Value;
        public override string ToString() => Value.ToString();
    }

    public class Add : Node
    {
        public Node Left { get; }
        public Node Right { get; }
        public Add(Node left, Node right) => (Left, Right) = (left, right);
        public void Deconstruct(out Node left, out Node right) => (left, right) = (Left, Right);
        public override string ToString() => $"({Left.ToString()} + {Right.ToString()})";
    }

    public class Mul : Node
    {
        public Node Left { get; }
        public Node Right { get; }
        public Mul(Node left, Node right) => (Left, Right) = (left, right);
        public void Deconstruct(out Node left, out Node right) => (left, right) = (Left, Right);
        public override string ToString() => $"{Left.ToString()} * {Right.ToString()}";
    }
}
