namespace SortVisualizer;

public partial class Sort
{
    static void Swap(ref int x, ref int y) => (x, y) = (y, x);
    static void Swap(int[] a, int i, int j) => Swap(ref a[i], ref a[j]);
}
