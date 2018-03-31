namespace ConsoleApp1.Constraints.Unmanaged
{
    /// <summary>
    /// プリミティブ型しか含まない構造体。
    /// unamanaged 型(ポインター化可能)。
    /// </summary>
    struct Point
    {
        public int X;
        public int Y;
        public int Z;
        public override string ToString() => (X, Y, Z).ToString();
    }
}
