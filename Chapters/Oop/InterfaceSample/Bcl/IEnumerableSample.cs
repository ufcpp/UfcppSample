namespace InterfaceSample.Bcl
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// 連結リスト。
    /// <see cref="IEnumerable{T}"/> を実装している = データの列挙ができる。複数のデータを束ねてる。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class LinkedList<T> : IEnumerable<T>
    {
        public T Value { get; }
        public LinkedList<T> Next { get; }

        public LinkedList(T value) : this(value, null) { }
        private LinkedList(T value, LinkedList<T> next) { Value = value; Next = next; }

        public LinkedList<T> Add(T value) => new LinkedList<T>(value, this);

        public IEnumerator<T> GetEnumerator()
        {
            if(Next != null)
                foreach (var x in Next)
                    yield return x;
            yield return Value;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    class IEnumerableSample
    {
        public static void Main()
        {
            var a = new LinkedList<int>(1);
            var b = a.Add(2).Add(3).Add(4);

            // foreach で使える(これは IEnumerable 必須ではない)
            foreach (var x in b)
                Console.WriteLine(x);

            // string.Join で使える
            Console.WriteLine(string.Join(", ", b));

            // LINQ で使える
            Console.WriteLine(b.Sum());
        }
    }
}
