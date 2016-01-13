using System;
using System.Collections;
using System.Collections.Generic;

namespace IteratorSample.Producer
{
    public class Program
    {
        public static void Run()
        {
            foreach (var x in ArrayEnumerable.Repeat(1, 5)) Console.WriteLine(x);
            foreach (var x in ManualEnumerable.Repeat(2, 5)) Console.WriteLine(x);
        }
    }

    public class IteratorEnumerable
    {
        public static IEnumerable<int> Repeat(int value, int count)
        {
            for (int i = 0; i < count; i++)
            {
                yield return value;
            }
        }
    }

    public class ArrayEnumerable
    {
        public static IEnumerable<int> Repeat(int value, int count)
        {
            var data = new int[count];
            for (int i = 0; i < count; i++)
            {
                data[i] = value;
            }
            return data;
        }
    }

    public class ManualEnumerable
    {
        struct Repeater : IEnumerable<int>, IEnumerator<int>
        {
            public int Current { get; }
            readonly int _count;
            int _i;

            public Repeater(int value, int count)
            {
                Current = value;
                _count = count;
                _i = 0;
            }

            public bool MoveNext()
            {
                _i++;
                return _i <= _count;
            }

            public IEnumerator<int> GetEnumerator()
                => _i == 0 ? this : new Repeater(Current, _count);

            object IEnumerator.Current => Current;
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
            public void Dispose() { }

            public void Reset()
            {
                throw new NotImplementedException();
            }
        }

        public static IEnumerable<int> Repeat(int value, int count)
            => new Repeater(value, count);
    }
}
