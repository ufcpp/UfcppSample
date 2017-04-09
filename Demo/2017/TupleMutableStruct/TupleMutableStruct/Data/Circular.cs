namespace TupleMutableStruct.Data
{
    using System;

    // 単位円周上の点
    struct Circular1
    {
        public double X { get; }
        public double Y { get; }
        public Circular1(double x, double y)
        {
            var abs = Math.Sqrt(x * x + y * y);
            X = x / abs;
            Y = y / abs;
        }
    }

    // 単位円周上の点なら、角度でデータを持てば double 1個で済む
    // 計算誤差を除けば Circular1 と全く同じ挙動を、半分のメモリ消費で実現している
    struct Circular2
    {
        private double _angle;
        public Circular2(double angle)
        {
            if (angle == 0) angle = 2 * Math.PI; // default 値の挙動を Circular1 と合わせるため
            _angle = angle;
        }
        public Circular2(double x, double y) : this(Math.Atan2(y, x)) { }

        public double X => _angle == 0 ? 0 : Math.Cos(_angle);
        public double Y => _angle == 0 ? 0 : Math.Sin(_angle);
    }
}
