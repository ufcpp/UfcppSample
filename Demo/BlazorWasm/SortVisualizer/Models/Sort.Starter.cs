namespace SortVisualizer;

public partial class Sort
{
    public static State Start(int[] array, Algorithm argorithm) => new(array, argorithm);

    public readonly struct State
    {
        public string Name { get; }
        private readonly int[] _array;
        private readonly IEnumerator<Operation> _sortOperations;

        public State(int[] array, Algorithm algorithm)
        {
            Name = algorithm.Name;
            _array = array;
            _sortOperations = algorithm.Sorter(array).GetEnumerator();
        }

        public ReadOnlySpan<int> Items => _array;
        public Operation Current => _sortOperations.Current;
        public bool MoveNext() => _sortOperations.MoveNext();
    }
}
