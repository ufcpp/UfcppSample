namespace UnionTypes
{
    /// <summary>
    /// 値と配列の直和。
    /// どっちか片方だけ値を持っている想定。
    /// </summary>
    interface IUnion<T>
    {
        T[] Array { get; }
        T Value { get; }
    }
}
