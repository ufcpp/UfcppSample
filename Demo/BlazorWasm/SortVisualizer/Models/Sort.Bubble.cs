namespace SortVisualizer;

public partial class Sort
{
    public static IEnumerable<Operation> BubbleSort(int[] a)
    {
        for (int i = 0; i < a.Length; i++)
            for (int j = 1; j < a.Length - i; j++)
            {
                yield return new(Kind.Compare, j, j - 1);
                if (a[j] < a[j - 1])
                {
                    Swap(a, j, j - 1);
                    yield return new(Kind.Swap, j, j - 1);
                }
            }
    }
}
