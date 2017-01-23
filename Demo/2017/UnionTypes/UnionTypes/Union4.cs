namespace UnionTypes
{
    /// <summary>
    /// 常に配列で持つ。
    /// 〇 as 不要
    /// × 値の時に常に配列の new 発生
    /// × 要素1の配列と値を区別したければ別途 isArray フラグが必要(これはこのフラグを**持たない**実装)
    /// </summary>
    struct Union4<T> : IUnion<T>
    {
        T[] _array;

        public T[] Array => _array;
        public T Value => _array.Length >= 1 ? _array[0] : default(T);

        public Union4(T value)
        {
            _array = new[] { value };
        }
        public Union4(T[] array)
        {
            _array = array;
        }
    }

    static class Union4
    {
        public static Union4<T> New<T>(T value) => new Union4<T>(value);
        public static Union4<T> New<T>(T[] array) => new Union4<T>(array);
    }
}
