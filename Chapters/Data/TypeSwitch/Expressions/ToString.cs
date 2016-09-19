using System;

namespace TypeSwitch.Expressions
{
    public static partial class NodeExtensions
    {
        public static string ToCsharpCode(this Node n)
        {
            switch (n)
            {
                case Var v: return "x";
                case Const c: return c.Value.ToString();
                case Add a: return a.Left.ToCsharpCode() + " + " + a.Right.ToCsharpCode();
                case Mul m: return m.Left.ToCsharpCode() + " * " + m.Right.ToCsharpCode();
            }
            throw new ArgumentOutOfRangeException();
        }
    }
}
