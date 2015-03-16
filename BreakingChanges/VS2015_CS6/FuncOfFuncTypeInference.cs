using System;

class FuncOfFuncTypeInference
{
    static void Main()
    {
        X(() => 10);
        X(() => () => 10);
    }

    private static T X<T>(Func<Func<T>> f) { return f()(); }
    private static T X<T>(Func<T> f) { return f(); }
}
