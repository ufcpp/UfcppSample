namespace MetricSpace
{
    public struct FloatArithmetic : IArithmetic<float>
    {
        public float MinValue => float.MinValue;
        public float MaxValue => float.MaxValue;
        public float Zero => 0;
        public float NegativeInfinity => float.NegativeInfinity;
        public float PositiveInfinity => float.PositiveInfinity;
        public float Add(float a, float b) => a + b;
        public float Subtract(float a, float b) => a - b;
        public float Multiply(float a, float b) => a * b;
    }
}
