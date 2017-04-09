namespace TupleMutableStruct.ValueObject.MutableStruct
{
    using System;
    using System.Collections.Generic;

    struct MutableStruct : IEquatable<MutableStruct>
    {
        public int X { get; set; }
        public int Y { get; set; }

        public bool Equals(MutableStruct other) => X == other.X && Y == other.Y;
        public override bool Equals(object obj) => obj is MutableStruct other && base.Equals(other);
        public override int GetHashCode() => X.GetHashCode() * 1234567 + Y.GetHashCode();
    }

    class Program
    {
        static void Main()
        {
            var map = new Dictionary<MutableStruct, int>();

            var key1 = new MutableStruct { X = 1, Y = 2 };
            map[key1] = 1;

            // 構造体の場合、Dictionary にはコピーが渡るので、こっちを書き換えても map には影響なし
            key1.X = 2;

            // 当然、key1では見つけられない
            Console.WriteLine(map.TryGetValue(key1, out _)); // False

            // 元のキーと同じ値を作る
            var key2 = new MutableStruct { X = 1, Y = 2 };

            // 見つかる！
            Console.WriteLine(map.TryGetValue(key2, out _)); // True
        }
    }

}
