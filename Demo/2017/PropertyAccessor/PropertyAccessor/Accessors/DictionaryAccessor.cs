using System.Collections.Generic;

namespace PropertyAccessor.Accessors
{
    public class DictionaryAccessor<T> : IAccessor<T>
    {
        private static Dictionary<string, EachCodeGenerator<T>.Entry> _accessors;

        static DictionaryAccessor()
        {
            _accessors = new Dictionary<string, EachCodeGenerator<T>.Entry>(EachCodeGenerator<T>.Items);
        }

        private T _value;
        public DictionaryAccessor(T value) => _value = value;

        public T Value => _value;
        public object this[string name]
        {
            get => _accessors[name].Get(ref _value);
            set => _accessors[name].Set(ref _value, value);
        }
    }
}
