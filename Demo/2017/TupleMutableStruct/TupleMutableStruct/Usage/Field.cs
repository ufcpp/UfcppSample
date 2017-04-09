namespace TupleMutableStruct.Usage.Field
{
    class Point : BindableBase
    {
        int x; // フィールドで直接保持
        int y;

        public int X { get => x; set => SetProperty(ref x, value); }
        public int Y { get => y; set => SetProperty(ref y, value); }
    }
}
