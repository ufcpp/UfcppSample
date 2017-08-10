using System.Collections.Generic;

namespace PropertyAccessor.Accessors
{
    public class SortedListAccessor<T> : IAccessor<T>
    {
        private static SortedList<string, EachCodeGenerator<T>.Entry> _accessors;

        static SortedListAccessor()
        {
            _accessors = new SortedList<string, EachCodeGenerator<T>.Entry>(EachCodeGenerator<T>.Items);
        }

        private T _value;
        public SortedListAccessor(T value) => _value = value;

        public T Value => _value;
        public object this[string name]
        {
            get => _accessors[name].Get(ref _value);
            set => _accessors[name].Set(ref _value, value);
        }
    }
}
