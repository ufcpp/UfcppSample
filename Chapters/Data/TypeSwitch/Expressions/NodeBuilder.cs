using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeSwitch.Expressions
{
    partial class Node
    {
        public static readonly Node X = new Var();
        public static implicit operator Node(int v) => new Const(v);
        public static Node operator+(Node l, Node r) => new Add(l, r);
        public static Node operator*(Node l, Node r) => new Mul(l, r);
    }
}
