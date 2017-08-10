namespace PropertyAccessor.Accessors
{
    public class SwitchAccessor<T> : IAccessor<T>
    {
        private T _value;
        public SwitchAccessor(T value) => _value = value;

        public T Value => _value;
        public object this[string name]
        {
            get => SwitchCodeGenerator<T>.Get(ref _value, name);
            set => SwitchCodeGenerator<T>.Set(ref _value, name, value);
        }
    }
}
