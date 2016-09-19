namespace TypeSwitch.Expressions
{
    abstract partial class Node
    {
        public abstract int Calculate(int x);
    }

    partial class Var
    {
        public override int Calculate(int x) => x;
    }

    partial class Const
    {
        public override int Calculate(int x) => Value;
    }

    partial class Add
    {
        public override int Calculate(int x) => Left.Calculate(x) + Right.Calculate(x);
    }

    partial class Mul
    {
        public override int Calculate(int x) => Left.Calculate(x) * Right.Calculate(x);
    }
}
