namespace InPlaceGroupBy;

public readonly ref struct SortedSpanGrouping<T>(Span<T> span, Comparison<T> comparison)
{
    private readonly Span<T> _span = span;
    private readonly Comparison<T> _comparison = comparison;

    public Enumerator GetEnumerator() => new(this);

    public ref struct Enumerator(SortedSpanGrouping<T> source)
    {
        private readonly SortedSpanGrouping<T> _source = source;
        private int _start;
        private int _end;

        public bool MoveNext()
        {
            if (_end >= _source._span.Length) return false;

            var first = _source._span[_end];
            _start = _end;
            while (true)
            {
                ++_end;
                if (_end >= _source._span.Length
                    || _source._comparison(first, _source._span[_end]) != 0) return true;
            }
        }

        public readonly Span<T> Current => _source._span[_start.._end];
    }
}
