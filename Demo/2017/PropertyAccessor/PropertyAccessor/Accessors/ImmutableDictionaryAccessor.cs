using System.Collections.Immutable;

namespace PropertyAccessor.Accessors
{
    public class ImmutableDictionaryAccessor<T> : IAccessor<T>
    {
        private static ImmutableDictionary<string, EachCodeGenerator<T>.Entry> _accessors;

        static ImmutableDictionaryAccessor()
        {
            _accessors = ImmutableDictionary<string, EachCodeGenerator<T>.Entry>.Empty.AddRange(EachCodeGenerator<T>.Items);
        }

        private T _value;
        public ImmutableDictionaryAccessor(T value) => _value = value;

        public T Value => _value;
        public object this[string name]
        {
            get => _accessors[name].Get(ref _value);
            set => _accessors[name].Set(ref _value, value);
        }
    }
}
