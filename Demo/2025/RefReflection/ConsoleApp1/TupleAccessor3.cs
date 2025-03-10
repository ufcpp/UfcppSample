namespace ConsoleApp1;

public readonly ref struct TupleAccessor<T1, T2, T3>(ref (T1, T2, T3) tuple) : ITupleAccessor
{
    private readonly ref (T1, T2, T3) _tuple = ref tuple;
    public readonly int Length => 3;
    public TypedRef this[int index] => index switch
    {
        0 => TypedRef.Create(ref _tuple.Item1),
        1 => TypedRef.Create(ref _tuple.Item2),
        2 => TypedRef.Create(ref _tuple.Item3),
        _ => throw new IndexOutOfRangeException(),
    };
}

partial class Accessor
{
    public static TupleAccessor<T1, T2, T3> Create<T1, T2, T3>(ref this (T1, T2, T3) tuple) => new(ref tuple);
}
