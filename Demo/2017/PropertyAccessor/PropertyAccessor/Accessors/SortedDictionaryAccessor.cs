using System.Collections.Generic;

namespace PropertyAccessor.Accessors
{
    public class SortedDictionaryAccessor<T> : IAccessor<T>
    {
        private static SortedDictionary<string, EachCodeGenerator<T>.Entry> _accessors;

        static SortedDictionaryAccessor()
        {
            _accessors = new SortedDictionary<string, EachCodeGenerator<T>.Entry>(EachCodeGenerator<T>.Items);
        }

        private T _value;
        public SortedDictionaryAccessor(T value) => _value = value;

        public T Value => _value;
        public object this[string name]
        {
            get => _accessors[name].Get(ref _value);
            set => _accessors[name].Set(ref _value, value);
        }
    }
}
