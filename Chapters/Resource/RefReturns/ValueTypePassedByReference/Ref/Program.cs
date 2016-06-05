namespace RefReturns.ValueTypePassedByReference.Ref
{
    class RefData
    {
        // 参照戻り値のプロパティで公開
        public ref Point P => ref _p;
        private Point _p;

        // 参照戻り値のインデクサーで公開
        public ref Point this[int i] => ref _items[i];
        private Point[] _items = new Point[3];
    }

    class Program
    {
        public static void Main()
        {
            var raw = new RefData();
            raw.P.X = 1; // プロパティ越しに、参照先のフィールドを書き換え可能
            raw[0].X = 1; // インデクサー越しに、参照先の配列を書き換え可能
        }
    }
}
