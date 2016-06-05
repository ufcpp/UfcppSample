namespace RefReturns.ValueTypePassedByReference.Raw
{
    class RawData
    {
        // フィールドを直接公開
        public Point P;

        // 配列を公開
        public Point[] Items { get; } = new Point[3];
    }

    class Program
    {
        public static void Main()
        {
            var raw = new RawData();
            raw.P.X = 1; // フィールドは直接書き換え可能
            raw.Items[0].X = 1; // 配列の要素の直接書き換え可能
        }
    }
}
