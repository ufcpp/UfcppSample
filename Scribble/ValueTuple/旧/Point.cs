namespace ValueTuples.旧
{
    public partial class Point : ITypedRecord, IDeepCloneable<Point>
    {
        ValueTuple<int, int> _value;

        public ValueTuple<int, int> Value => _value;

        ITuple IRecord.Value { get { return _value; } set { _value = (ValueTuple<int, int>)value; } }
        IRecordInfo ITypedRecord.GetInfo() => PointInfo.Instance;

        Point IDeepCloneable<Point>.Clone() => new Point(_value.DeepClone());

        public Point() { }
        public Point(int x, int y) { X = x; Y = y; }
        public Point(ValueTuple<int, int> value) { _value = value; }

        public int X { get { return _value.Item1; } set { _value.Item1 = value; } }
        public int Y { get { return _value.Item2; } set { _value.Item2 = value; } }

        public override string ToString() => $"({X}, {Y})";
    }

    internal class PointInfo : IRecordInfo
    {
        public static readonly IRecordInfo Instance = new PointInfo();

        private static readonly string[] _keys = new[] { "x", "y" };

        string IRecordInfo.GetKey(int index) => _keys[index];

        int IRecordInfo.GetIndex(string key)
        {
            switch (key)
            {
                default: return -1;
                case "x": return 0;
                case "y": return 1;
            }
        }

        object IRecordInfo.NewInstance(int index, int? discriminator) => null;

        int? IRecordInfo.Discriminator => null;
    }
}
