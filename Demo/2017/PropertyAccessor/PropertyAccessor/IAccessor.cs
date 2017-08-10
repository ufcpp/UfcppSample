namespace PropertyAccessor
{
    interface IAccessor<T>
    {
        T Value { get; }
        object this[string name] { get; set; }
    }
}
