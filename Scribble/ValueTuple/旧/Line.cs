namespace ValueTuples.旧
{
    public partial class Line : ITypedRecord, IDeepCloneable<Line>
    {
        ValueTuple<Point, Point> _value;

        public ValueTuple<Point, Point> Value => _value;

        ITuple IRecord.Value { get { return _value; } set { _value = (ValueTuple<Point, Point>)value; } }
        IRecordInfo ITypedRecord.GetInfo() => LineInfo.Instance;

        Line IDeepCloneable<Line>.Clone() => new Line(_value.DeepClone());

        public Line() { }
        public Line(Point a, Point b) { A = a; B = b; }
        public Line(ValueTuple<Point, Point> value) { _value = value; }

        public Point A { get { return _value.Item1; } set { _value.Item1 = value; } }
        public Point B { get { return _value.Item2; } set { _value.Item2 = value; } }

        public override string ToString() => $"{A} - {B}";
    }


    internal class LineInfo : IRecordInfo
    {
        public static readonly IRecordInfo Instance = new LineInfo();

        private static readonly string[] _keys = new[] { "a", "b" };

        string IRecordInfo.GetKey(int index) => _keys[index];

        int IRecordInfo.GetIndex(string key)
        {
            switch (key)
            {
                default: return -1;
                case "a": return 0;
                case "b": return 1;
            }
        }

        object IRecordInfo.NewInstance(int index, int? discriminator)
        {
            switch (index)
            {
                default: return null;
                case 0: return new Point();
                case 1: return new Point();
            }
        }

        int? IRecordInfo.Discriminator => null;
    }
}
