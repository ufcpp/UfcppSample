using System;
using System.Linq;

namespace RgiSequenceFinder
{
    /// <summary>
    /// RGI 絵文字シーケンスの辞書化、かなり条件を限定できるので専用のハッシュテーブルを作る。
    /// </summary>
    /// <remarks>
    /// 条件:
    /// - たかだか4000文字程度(Unicode 13.0 で3300文字)
    /// - 半分以上が <see cref="CharDictionary"/> に流せる
    /// - 最長の文字数もわかってる(Unicode 13.0 で UTF-16 14文字)
    ///
    /// なので、
    /// - 文字列リテラルは全部連結したものを1個だけ渡す
    ///   - キーの代わりに文字数一覧を渡す
    /// - バケットを最初に固定長で取って、以後、resize 一切なし
    /// - GetHashCode の実装も「16文字以下」みたいな条件で計算する
    /// </remarks>
    class StringDictionary
    {
        /// <summary>
        /// 辞書容量。
        /// RGI 絵文字シーケンスが3300文字程度、その半分以上が <see cref="CharDictionary"/> 行きでこっちには来ないので、残り1700文字程度。
        /// 12ビット(4096)あれば十分。
        /// </summary>
        private const int Bits = 12;
        private const int Capacity = 1 << Bits;
        private const int Mask = Capacity - 1;

        private static ReadOnlySpan<byte> Primes => new byte[] { 83, 223, 227, 131, 53, 149, 227, 229, 7, 23, 5, 47, 59, 53, 3, 29 };
        private const int Skip = 655883; // 適当な大き目の素数

        private static int GetHashCode(ReadOnlySpan<char> s)
        {
            var primes = Primes;
            var hash = 0;
            for (int i = 0; i < s.Length && i < primes.Length; i++)
            {
                hash += s[i] * primes[i];
            }
            return hash;
        }

        internal struct Bucket
        {
            public ushort KeyStart;
            public ushort KeyLength; // 空文字列がこないので、これの == 0 で空判定。
            public ushort Value;
            public bool HasValue => KeyLength != 0;
        }

        private readonly string _concatinatedString;
        private readonly Bucket[] _buckets = new Bucket[Capacity];

        private StringDictionary() { _concatinatedString = null!; }

        public StringDictionary(string concatinatedString, ReadOnlySpan<byte> lengths, ReadOnlySpan<ushort> indexes)
        {
            if (lengths.Length != indexes.Length) throw new ArgumentException("length mismatched");

            _concatinatedString = concatinatedString;

            var span = concatinatedString.AsSpan();
            ushort total = 0;
            for (int i = 0; i < lengths.Length; i++)
            {
                var len = lengths[i];
                Add(span, total, len, indexes[i]);
                total += len;
            }
        }

        /// <summary>
        /// 要素の追加。
        /// </summary>
        /// <remarks>
        /// 最初に想定している以上に追加すると永久ループする可能性があるので注意。
        /// (<see cref="CompactDictionary{TKey, TValue, TComparer}.CompactDictionary(int)"/>の引数で与えた数字の2倍を超えると可能性あり)
        /// </remarks>
        public void Add(ReadOnlySpan<char> s, ushort keyStart, ushort keyLength, ushort value)
        {
            var key = s.Slice(keyStart, keyLength);
            var hash = GetHashCode(key) & Mask;
            var buckets = _buckets;

            while (true)
            {
                ref var b = ref buckets[hash];

                if (!b.HasValue)
                {
                    b.KeyStart = keyStart;
                    b.KeyLength = keyLength;
                    b.Value = value;
                    break;
                }

                hash = (hash + Skip) & Mask;
            }
        }

        /// <summary>
        /// 値の取得。
        /// キーが見つからなかったら false を返す。
        /// </summary>
        public bool TryGetValue(ReadOnlySpan<char> key, out ushort value)
        {
            var hash = GetHashCode(key) & Mask;
            var buckets = _buckets;

            while (true)
            {
                var b = buckets[hash];

                if (!b.HasValue)
                {
                    value = 0;
                    return false;
                }
                else if (b.KeyLength != 0 && key.SequenceEqual(_concatinatedString.AsSpan(b.KeyStart, b.KeyLength)))
                {
                    value = b.Value;
                    return true;
                }

                hash = (hash + Skip) & Mask;
            }
        }
    }
}
