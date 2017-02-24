namespace HeapAllocation.Data
{
    /// <summary>
    /// <see cref="StructPoint"/>をクラスに変えるとどのくらい遅くなるか試す用のクラス。
    /// </summary>
    class ClassPoint
    {
        public int X { get; }
        public int Y { get; }
        public ClassPoint(int x, int y) => (X, Y) = (x, y);
    }
}
