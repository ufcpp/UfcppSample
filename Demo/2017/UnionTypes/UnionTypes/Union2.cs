namespace UnionTypes
{
    /// <summary>
    /// object に格納するやり方。
    /// 〇 フィールドが1個
    /// × 値型の時にbox化が起きる
    /// × 値の取り出し時にも as (isinst命令)のコスト
    /// </summary>
    struct Union2<T> : IUnion<T>
    {
        object _value;

        public T[] Array => _value as T[];
        public T Value => (T)_value;

        public Union2(T value) => _value = value;
        public Union2(T[] array) => _value = array;
    }

    static class Union2
    {
        public static Union2<T> New<T>(T value) => new Union2<T>(value);
        public static Union2<T> New<T>(T[] array) => new Union2<T>(array);
    }
}
