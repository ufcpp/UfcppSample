using System.Collections;
using System.Collections.Generic;

struct Warning1<T> : IEnumerable<T>
{
    IEnumerator<T> IEnumerable<T>.GetEnumerator() => null!; // CS8616
    IEnumerator IEnumerable.GetEnumerator() => null!;
}

struct Warning2<T> : IEnumerable<T> where T : class ?
{
    IEnumerator<T> IEnumerable<T>.GetEnumerator() => null!; // CS8616
    IEnumerator IEnumerable.GetEnumerator() => null!;
}

struct NoWarning1 : IEnumerable<int>
{
    IEnumerator<int> IEnumerable<int>.GetEnumerator() => null!;
    IEnumerator IEnumerable.GetEnumerator() => null!;
}

struct NoWarning2<T> : IEnumerable<T> where T : class
{
    IEnumerator<T> IEnumerable<T>.GetEnumerator() => null!;
    IEnumerator IEnumerable.GetEnumerator() => null!;
}

struct NoWarning3<T> : IEnumerable<T> where T : struct
{
    IEnumerator<T> IEnumerable<T>.GetEnumerator() => null!;
    IEnumerator IEnumerable.GetEnumerator() => null!;
}
