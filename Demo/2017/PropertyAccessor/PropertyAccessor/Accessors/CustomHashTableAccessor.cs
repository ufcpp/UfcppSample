using System.Linq;

namespace PropertyAccessor.Accessors
{
    /// <summary>
    /// プロパティが後から増えることはないんだから、最初にハッシュ値被りがない範囲で最小のサイズの配列を固定長で取ってしまうという、
    /// メモリ効率をある程度妥協、「プロパティ数なんてたかが知れてるだろ」前提のハッシュテーブル。
    ///
    /// あと、この実装では、キーのチェックしないという妥協もしてる。
    /// 要するに、
    /// - 最初にハッシュ値被りがないように作ったんだから、ちゃんと存在するプロパティ名で値を引くことは正確にできる
    /// - 一方で、存在しないプロパティ名で検索して、ハッシュ値が被ってる別のプロパティの値が取れてしまうことはあり得る
    /// </summary>
    /// <remarks>
    /// 計測一例:
    ///           Method |       Mean |     Error |    StdDev |  Gen 0 | Allocated |
    /// ---------------- |-----------:|----------:|----------:|-------:|----------:|
    ///       ItemSwitch |   479.7 ns |  6.655 ns |  5.900 ns | 0.0639 |     272 B |
    ///       ItemCustom |   991.6 ns | 36.094 ns | 37.066 ns | 0.0629 |     272 B |
    ///   ItemDictionary | 1,006.4 ns |  5.293 ns |  4.693 ns | 0.0629 |     272 B |
    ///      PointSwitch |   253.2 ns |  1.395 ns |  1.305 ns | 0.0434 |     184 B |
    ///      PointCustom |   280.3 ns |  1.409 ns |  1.318 ns | 0.0434 |     184 B |
    ///  PointDictionary |   397.9 ns |  3.329 ns |  3.114 ns | 0.0434 |     184 B |
    ///
    /// 結構、高速化のために強い制限掛けて実装してる割には <see cref="System.Collections.Generic.Dictionary{TKey, TValue}"/>との差がそんなに大きくない。
    /// </remarks>
    public class CustomHashTableAccessor<T> : IAccessor<T>
    {
        private static int _mask;
        private static EachCodeGenerator<T>.Entry[] _array;

        static CustomHashTableAccessor()
        {
            var items = EachCodeGenerator<T>.Items;

            var mask = 0;
            for (int i = 1; i < items.Count; i <<= 1) mask = (mask << 1) | 1;

            var hashCodes = items.Keys.Select(k => k.GetHashCode()).ToArray();

            for (int i = 0; i < 32; i++)
            {
                if (hashCodes.Select(x => x & mask).Distinct().Count() == hashCodes.Length) break;
                mask = (mask << 1) | 1;
            }

            _array = new EachCodeGenerator<T>.Entry[mask + 1];

            foreach (var (key, value) in items)
            {
                _array[key.GetHashCode() & mask] = value;
            }

            _mask = mask;
        }

        private T _value;
        public CustomHashTableAccessor(T value) => _value = value;

        public T Value => _value;
        public object this[string name]
        {
            get => _array[name.GetHashCode() & _mask].Get(ref _value);
            set => _array[name.GetHashCode() & _mask].Set(ref _value, value);
        }
    }
}
