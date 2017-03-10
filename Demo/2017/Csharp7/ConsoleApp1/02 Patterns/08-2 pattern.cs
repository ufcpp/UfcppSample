namespace ConsoleApp1._02_Patterns
{
    // 拡張メソッド + 型スイッチで実装する例
    // 〇 クラスの外に書ける/別の人が書ける
    // × クラスに変更があったとき、このメソッドの実装者が変更に気づきにくい
    // × 多少オーバーヘッドあり
    static class NodeExtensions
    {
        public static Node Reduce(this Node n)
        {
            // 一か所にコードが集まってることが利点になることもある。コード分量次第。ｚ
            switch (n)
            {
                case Add a:
                    {
                        var x = a.X.Reduce();
                        var y = a.Y.Reduce();

                        if (x is Const cx)
                            if (cx.Value == 0) return y;
                            else if (y is Const cy1) return new Const(cx.Value + cy1.Value);
                        if (y is Const cy)
                            if (cy.Value == 0) return x;
                            else if (x is Const cx1) return new Const(cx1.Value + cy.Value);
                        return new Add(x, y);
                    }
                case Mul m:
                    {
                        var x = m.X.Reduce();
                        var y = m.Y.Reduce();

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

                default:
                    return n;
            }
        }
    }
}
