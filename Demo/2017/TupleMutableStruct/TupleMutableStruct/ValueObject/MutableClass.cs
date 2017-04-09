namespace TupleMutableStruct.ValueObject.MutableClass
{
    using System;

    class MutableClass : IEquatable<MutableClass>
    {
        public int X { get; set; }
        public int Y { get; set; }

        public bool Equals(MutableClass other) => X == other.X && Y == other.Y;
        public override bool Equals(object obj) => obj is MutableClass other && base.Equals(other);
        public override int GetHashCode() => X.GetHashCode() * 1234567 + Y.GetHashCode();
    }
}

namespace TupleMutableStruct.ValueObject.MutableClass
{
    using System;
    using System.Collections.Generic;

    class Program
    {
        static void Main()
        {
            var map = new Dictionary<MutableClass, int>();

            var key1 = new MutableClass { X = 1, Y = 2 };
            map[key1] = 1;

            // キーにしたインスタンスを書き換えてしまう
            key1.X = 2;

            // 当然、見つからなくなる
            Console.WriteLine(map.TryGetValue(key1, out _)); // False

            // 元のキーと同じ値の、別インスタンスを作る
            var key2 = new MutableClass { X = 1, Y = 2 };

            // これでも、見つからなくなる
            Console.WriteLine(map.TryGetValue(key2, out _)); // False

        }
    }
}