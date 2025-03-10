namespace ConsoleApp1;

public readonly ref struct TupleAccessor<T1, T2>(ref (T1, T2) tuple) : ITupleAccessor
{
    private readonly ref (T1, T2) _tuple = ref tuple;
    public readonly int Length => 2;
    public TypedRef this[int index] => index switch
    {
        0 => TypedRef.Create(ref _tuple.Item1),
        1 => TypedRef.Create(ref _tuple.Item2),
        _ => throw new IndexOutOfRangeException(),
    };
}

partial class Accessor
{
    public static TupleAccessor<T1, T2> Create<T1, T2>(ref this (T1, T2) tuple) => new(ref tuple);
}
