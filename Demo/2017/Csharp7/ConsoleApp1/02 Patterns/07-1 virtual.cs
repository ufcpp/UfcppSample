namespace ConsoleApp1._02_Patterns
{
    // 仮想メソッドで実装する例
    // 〇 それなりに高速
    // 〇 クラスに変更があったとき、このメソッドの実装者が変更に気づきやすい
    // × クラスの中にないといけない/クラス実装者にしか足せない
    abstract partial class Node
    {
        public virtual Node Reduce() => this;
    }

    partial class Add : Node
    {
        public override Node Reduce()
        {
            var x = X.Reduce();
            var y = Y.Reduce();

            if (x is Const cx)
                if (cx.Value == 0) return y;
                else if (y is Const cy1) return new Const(cx.Value + cy1.Value);
            if (y is Const cy)
                if (cy.Value == 0) return x;
                else if (x is Const cx1) return new Const(cx1.Value + cy.Value);
            return new Add(x, y);
        }
    }

    partial class Mul : Node
    {
        public override Node Reduce()
        {
            var x = X.Reduce();
            var y = Y.Reduce();

            if (x is Const cx)
                if (cx.Value == 0) return new Const(0);
                else if (cx.Value == 1) return y;
                else if (y is Const cy1) return new Const(cx.Value * cy1.Value);
            if (y is Const cy)
                if (cy.Value == 0) return new Const(0);
                else if (cy.Value == 1) return x;
                else if (x is Const cx1) return new Const(cx1.Value * cy.Value);
            return new Mul(x, y);
        }
    }
}
