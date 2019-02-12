namespace Patterns.Expressions
{
    using System;

    public static class C
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
                // 0 を足しても変わらない
                (Const (0), var r1) => r1,
                (var l1, Const (0)) => l1,
                // 他
                (var l1, var r1) => new Add(l1, r1)
            },
            Mul (var l, var r) => (l.Simplify(), r.Simplify()) switch
            {
                // 0 を掛けたら 0
                (Const (0) c, _) => c,
                (_, Const (0) c) => c,
                // 1 を掛けても変わらない
                (Const (1), var r1) => r1,
                (var l1, Const (1)) => l1,
                // 他
                (var l1, var r1) => new Mul(l1, r1)
            },
            _ => n
        };

        public static Node ClassicSimplify(this Node n)
        {
            if (n is Add a)
            {
                var (l, r) = a;
                var l1 = l.Simplify();
                var r1 = r.Simplify();

                { if (l1 is Const c && c.Value == 0) return r1; }
                { if (r1 is Const c && c.Value == 0) return l1; }
                return new Add(l1, r1);
            }
            if (n is Mul m)
            {
                var (l, r) = m;
                var l1 = l.Simplify();
                var r1 = r.Simplify();

                {
                    if (l1 is Const c)
                    {
                        if (c.Value == 0) return c;
                        if (c.Value == 1) return r1;
                    }
                }
                {
                    if (r1 is Const c)
                    {
                        if (c.Value == 0) return c;
                        if (c.Value == 1) return l1;
                    }
                }
                return new Mul(l1, r1);
            }
            return n;
        }

        static void Main()
        {
            var x = Node.X;
            Console.WriteLine(x * x + 1);
            Console.WriteLine(((1 * x + 0 * x) * 1).Simplify());
            Console.WriteLine(((x + 2) * (x + 3)).Calculate(1));
        }
    }
}
