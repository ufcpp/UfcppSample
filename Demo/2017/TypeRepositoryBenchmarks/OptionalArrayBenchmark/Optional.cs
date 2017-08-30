using System;

struct Optional<T>
{
    public bool HasValue { get; }
    private T _value;

    internal Optional(T value, bool hasValue)
    {
        _value = value;
        HasValue = hasValue;
    }
    public Optional(T value) : this(value, true) { }
    public static readonly Optional<T> None = default(Optional<T>);

    public T Value => HasValue ? _value : throw new NullReferenceException();
    public T GetValueOrDefault() => _value;
}
