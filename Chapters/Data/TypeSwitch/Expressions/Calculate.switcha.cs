using System;

namespace TypeSwitch.Expressions
{
    public static partial class NodeExtensions
    {
        public static int Calculate(this Node n, int x)
        {
            switch (n)
            {
                case Var v: return x;
                case Const c: return c.Value;
                case Add a: return Calculate(a.Left, x) + Calculate(a.Right, x);
                case Mul m: return Calculate(m.Left, x) * Calculate(m.Right, x);
            }
            throw new ArgumentOutOfRangeException();
        }
    }
}
