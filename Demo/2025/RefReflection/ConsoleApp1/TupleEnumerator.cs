namespace ConsoleApp1;

public ref struct TupleEnumerator<TAccessor>(TAccessor tuple)
    where TAccessor : struct, ITupleAccessor, allows ref struct
{
    private TAccessor _tuple = tuple;
    private int _index = -1;

    public bool MoveNext()
    {
        return ++_index < _tuple.Length;
    }
    public TypedRef Current => _tuple[_index];
}

public static partial class Accessor
{
    public static TupleEnumerator<TAccessor> GetEnumerator<TAccessor>(this TAccessor tuple)
       where TAccessor : struct, ITupleAccessor, allows ref struct
        => new(tuple);
}
