namespace UnionTypes
{
    /// <summary>
    /// 常に配列で持つ。
    /// 〇 as 不要
    /// × 値の時に常に配列の new 発生
    /// × 要素1の配列と値を区別したければ別途 isArray フラグが必要(これはこのフラグを**持つ**実装)
    /// </summary>
    struct Union5<T> : IUnion<T>
    {
        bool _isArray;
        T[] _array;

        public T[] Array => _isArray ? _array : null;
        public T Value => _array[0];

        public Union5(T value)
        {
            _isArray = false;
            _array = new[] { value };
        }
        public Union5(T[] array)
        {
            _isArray = true;
            _array = array;
        }
    }

    static class Union5
    {
        public static Union5<T> New<T>(T value) => new Union5<T>(value);
        public static Union5<T> New<T>(T[] array) => new Union5<T>(array);
    }
}
