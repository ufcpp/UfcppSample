namespace SortVisualizer;

public partial class Sort
{
    public static IReadOnlyList<Algorithm> Algorithms => _algorithms;

    private static readonly Algorithm[] _algorithms = new Algorithm[]
    {
        new("Bubble Sort", BubbleSort),
        new("Quick Sort", QuickSort),
    };

    public record struct Algorithm(string Name, Func<int[], IEnumerable<Operation>> Sorter)
    {
        public State Start(int[] array) => new(array, this);
    }
}
