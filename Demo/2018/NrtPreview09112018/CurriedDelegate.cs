using System;
using System.Collections.Generic;

static class Ex
{
    public static void Add<T1, T2>(this List<(T1, T2)> list, T1 x1, T2 x2) => list.Add((x1, x2));
}

class CurriedDelegate
{
    static void Main()
    {
        var list = new List<(int, int)>();
        Action<(int, int)> ok1 = list.Add;
        Action<int, int> ok2 = (x, y) => list.Add((x, y));

        Action<int, int> warning = list.Add; // Warning CS8622  Nullability of reference types in type of parameter 'list' of 'void Ex.Add<int, int>(List<(int, int)> list, int x1, int x2)' doesn't match the target delegate 'A<int, int>'.
    }
}
