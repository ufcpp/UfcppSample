namespace ConsoleApp1.Ref
{
    /// <summary>
    /// 配列を
    /// foreach (ref var item in array)
    /// でループするための enumerable。
    /// </summary>
    struct RefArrayEnumerable<T>
    {
        T[] _array;
        public RefArrayEnumerable(T[] array) => _array = array;
        public RefArrayEnumerator<T> GetEnumerator() => new RefArrayEnumerator<T>(_array);
    }

    struct RefArrayEnumerator<T>
    {
        int _index;
        T[] _array;
        public RefArrayEnumerator(T[] array) => (_index, _array) = (-1, array);
        public ref T Current => ref _array[_index];
        public bool MoveNext() => ++_index < _array.Length;
    }
}
