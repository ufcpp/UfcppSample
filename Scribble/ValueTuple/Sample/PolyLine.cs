using System;
using System.Collections.Generic;
using System.Linq;
using ValueTuples.Reflection;

namespace ValueTuples.Sample
{
    public partial class PolyLine
    {
        public override string ToString() => string.Join(" - ", Points.AsEnumerable()) + " / " + string.Join(", ", Weights.AsEnumerable());
    }

    public partial class PolyLine
    {
        public Point[] Points { get; set; }
        public double[] Weights { get; set; }

        public PolyLine() { }
        public PolyLine(Point[] points, double[] weights) { Points = points; Weights = weights; }

        static PolyLine()
        {
            TypeRepository.Register(typeof(PolyLine), new PolyLineInfo());
        }
    }

    internal struct PolyLineAccessor : IRecordAccessor
    {
        PolyLine _value;

        public PolyLineAccessor(PolyLine value) { _value = value; }

        public int Count => 2;

        public object Get(string key)
        {
            switch (key)
            {
                case "Points": return _value.Points;
                case "Weights": return _value.Weights;
                default: return null;
            }
        }

        public object Get(int index)
        {
            switch (index)
            {
                case 0: return _value.Points;
                case 1: return _value.Weights;
                default: return null;
            }
        }

        public void Set(string key, object value)
        {
            switch (key)
            {
                case "Points": _value.Points = (Point[])value; break;
                case "Weights": _value.Weights = (double[])value; break;
            }
        }

        public void Set(int index, object value)
        {
            switch (index)
            {
                case 0: _value.Points = (Point[])value; break;
                case 1: _value.Weights = (double[])value; break;
            }
        }
    }


    internal class PolyLineInfo : RecordTypeInfo
    {
        public override Type Type => typeof(PolyLine);

        private static readonly RecordFieldInfo[] _fields =
        {
            new RecordFieldInfo(TypeRepository.Get(typeof(Point[])), "Points", 0),
            new RecordFieldInfo(TypeRepository.Get(typeof(double[])), "Weights", 1),
        };

        public override IEnumerable<RecordFieldInfo> Fields => _fields;

        public override object GetInstance() => new PolyLine();
        public override Array GetArray(int length) => new PolyLine[length];

        public override IRecordAccessor GetAccessor(object instance) => new PolyLineAccessor((PolyLine)instance);
    }
}
