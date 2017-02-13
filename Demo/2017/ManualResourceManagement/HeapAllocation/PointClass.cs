namespace HeapAllocation
{
    /// <summary>
    /// <see cref="PointStruct"/>をクラスに変えるとどのくらい遅くなるか試す用のクラス。
    /// </summary>
    class PointClass
    {
        public int X { get; }
        public int Y { get; }
        public PointClass(int x, int y) => (X, Y) = (x, y);
    }
}
