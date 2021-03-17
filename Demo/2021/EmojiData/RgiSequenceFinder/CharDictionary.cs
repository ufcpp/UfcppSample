using System;

namespace RgiSequenceFinder
{
    /// <summary>
    /// char → インデックスの辞書。
    /// </summary>
    /// <remarks>
    /// RGI 絵文字、そもそも単体の符号点とか、単体 + FE0F が多いので char → インデックスの辞書を持った方が効率が良かった。
    /// 元が基本的に連番なので、ハッシュ値として単に下位ビットだけ使ってもほとんど被らない。
    /// </remarks>
    class CharDictionary
    {
        // 元が連番のものをハッシュ値にしてるので、「被ったら固定値ずらす」方針は同じ被り方が続いて O(1) にならないかも。
        // 要調査。
        private const int Skip = 655883;

        internal struct Bucket
        {
            public char Key; // ヌル文字が来ないことわかってるので == 0 で空判定 OK。
            public ushort Value;
        }

        private readonly int _mask;
        private readonly Bucket[] _buckets;

        public CharDictionary(int capacity, ReadOnlySpan<char> keys, ReadOnlySpan<ushort> indexes)
        {
            //todo: capacity が2のべき乗になってるか確認

            if (keys.Length != indexes.Length) throw new ArgumentException("length mismatched");
            _buckets = new Bucket[capacity];
            _mask = capacity - 1;

            for (int i = 0; i < keys.Length; i++)
            {
                Add(keys[i], indexes[i]);
            }
        }

        /// <summary>
        /// 要素の追加。
        /// </summary>
        /// <remarks>
        /// 最初に想定している以上に追加すると永久ループする可能性があるので注意。
        /// </remarks>
        public void Add(char key, ushort value)
        {
            var mask = _mask;
            var hash = key & mask;
            var buckets = _buckets;

            while (true)
            {
                ref var b = ref buckets[hash];

                if (b.Key == 0)
                {
                    b.Key = key;
                    b.Value = value;
                    break;
                }

                hash = (hash + Skip) & mask;
            }
        }

        /// <summary>
        /// 値の取得。
        /// キーが見つからなかったら false を返す。
        /// </summary>
        public bool TryGetValue(char key, out ushort value)
        {
            var mask = _mask;
            var hash = key & mask;
            var buckets = _buckets;

            while (true)
            {
                var b = buckets[hash];

                if (b.Key == 0)
                {
                    value = 0;
                    return false;
                }
                else if (b.Key == key)
                {
                    value = b.Value;
                    return true;
                }

                hash = (hash + Skip) & mask;
            }
        }
    }
}
