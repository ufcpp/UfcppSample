namespace TupleMutableStruct.Usage.Tuple
{
    class Point : BindableBase
    {
        (int x, int y) t; // タプルにして保持

        public int X { get => t.x; set => SetProperty(ref t.x, value); }
        public int Y { get => t.y; set => SetProperty(ref t.y, value); }
    }
}
