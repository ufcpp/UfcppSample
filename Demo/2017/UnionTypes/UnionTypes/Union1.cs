namespace UnionTypes
{
    /// <summary>
    /// 値と配列、2つのフィールドを持つやり方。
    /// 〇 ボックス化起きない
    /// × フィールドが2個でコピーコストが高い
    /// </summary>
    struct Union1<T> : IUnion<T>
    {
        T _value;
        T[] _array;

        public T[] Array => _array;
        public T Value => _value;

        public Union1(T value)
        {
            _value = value;
            _array = null;
        }

        public Union1(T[] array)
        {
            _value = default(T);
            _array = array;
        }
    }

    static class Union1
    {
        public static Union1<T> New<T>(T value) => new Union1<T>(value);
        public static Union1<T> New<T>(T[] array) => new Union1<T>(array);
    }
}
