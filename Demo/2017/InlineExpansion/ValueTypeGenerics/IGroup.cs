namespace ValueTypeGenerics
{
    /// <summary>
    /// 零元と加算演算子がある集合。
    /// </summary>
    public interface IGroup
    {
        int Zero { get; }
        int Op(int x, int y);
    }

    /// <summary>
    /// 加法群。
    /// </summary>
    public struct AddGroup : IGroup
    {
        public int Zero => 0;
        public int Op(int x, int y) => x + y;
    }

    /// <summary>
    /// 乗法群。
    /// </summary>
    public struct MulGroup : IGroup
    {
        public int Zero => 1;
        public int Op(int x, int y) => x * y;
    }
}
