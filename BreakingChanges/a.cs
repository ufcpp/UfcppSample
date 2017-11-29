using System;

interface X<in U, out T>
{
    Func<U, T> X(Func<T, U> f);
}

class P
{
    static void Main() { }
}