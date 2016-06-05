namespace RefReturns.ValueTypePassedByReference.Capsuled
{
    class CapsuledData
    {
        // プロパティで公開
        public Point P { get; set; }

        // インデクサーで公開
        public Point this[int i]
        {
            get { return _items[i]; }
            set { _items[i] = value; }
        }
        private Point[] _items = new Point[3];
    }

    class Program
    {
        public static void Main()
        {
#if false
            var cap = new CapsuledData();
            cap.P.X = 1; // プロパティの戻り値(コピー品)の書き換えはエラーに
            cap[0].X = 1; // インデクサーの戻り値も同様、エラーに
#else
            // こんな書き方が必須になる
            var cap = new CapsuledData();
            var p = cap.P;
            p.X = 1;
            cap.P = p;
            var q = cap[0];
            q.X = 1;
            cap[0] = q;
#endif
        }
    }
}
