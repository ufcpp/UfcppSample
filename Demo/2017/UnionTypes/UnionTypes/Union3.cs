namespace UnionTypes
{
    /// <summary>
    /// virtual な実装。
    /// 〇 as とかはなくなる
    /// × 常にヒープ確保
    /// </summary>
    /// <typeparam name="T"></typeparam>
    abstract class Union3<T> : IUnion<T>
    {
        public abstract T[] Array { get; }
        public abstract T Value { get; }
        private Union3() { }

        public class UnionValue : Union3<T>
        {
            public override T[] Array => null;
            public override T Value { get; }
            public UnionValue(T value) => Value = value;
        }

        public class UnionArray : Union3<T>
        {
            public override T[] Array { get; }
            public override T Value => default(T);
            public UnionArray(T[] array) => Array = array;
        }

        public static Union3<T> New(T value) => new UnionValue(value);
        public static Union3<T> New(T[] array) => new UnionArray(array);
    }

    static class Union3
    {
        public static Union3<T> New<T>(T value) => Union3<T>.New(value);
        public static Union3<T> New<T>(T[] array) => Union3<T>.New(array);
    }
}
