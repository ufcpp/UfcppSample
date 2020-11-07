namespace MetricSpace
{
    public struct DoubleArithmetic : IArithmetic<double>
    {
        public double MinValue => double.MinValue;
        public double MaxValue => double.MaxValue;
        public double Zero => 0;
        public double NegativeInfinity => double.NegativeInfinity;
        public double PositiveInfinity => double.PositiveInfinity;
        public double Add(double a, double b) => a + b;
        public double Subtract(double a, double b) => a - b;
        public double Multiply(double a, double b) => a * b;
    }
}
