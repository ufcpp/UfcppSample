namespace Preview2.RecursivePattern
{
    using System;

    public class Program
    {
        static void Main()
        {
            var x = Node.X;
            Console.WriteLine(x * x + 1);
            Console.WriteLine(((1 * x + 0 * x) * 1).Simplify());
            Console.WriteLine(((x + 2) * (x + 3)).Calculate(1));
        }
    }

    public static class NodeExtensions
    {
        public static int Calculate(this Node n, int x)
            => n switch
        {
            Var _ => x,
            Const c => c.Value,
            Add (var l, var r) => l.Calculate(x) + r.Calculate(x),
            Mul (var l, var r) => l.Calculate(x) * r.Calculate(x),
            _ => throw new InvalidOperationException()
        };

        public static Node Simplify(this Node n)
            => n switch
        {
            Add (var l, var r) => (l.Simplify(), r.Simplify()) switch
            {
                (Const (0), var r1) => r1,
                (var l1, Const (0)) => l1,
                (var l1, var r1) => new Add(l1, r1)
            },
            Mul (var l, var r) => (l.Simplify(), r.Simplify()) switch
            {
                (Const (0) c, _) => c,
                (_, Const (0) c) => c,
                (Const (1), var r1) => r1,
                (var l1, Const (1)) => l1,
                (var l1, var r1) => new Mul(l1, r1)
            },
            _ => n
        };
    }

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
