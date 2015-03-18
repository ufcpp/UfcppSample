using System;

class FuncOfFuncTypeInference
{
    static void Main()
    {
        X(() => () => 10);
        Y(() => () => 10);
    }

    private static int X(Func<Func<int>> f) { return f()(); }
    private static int X(Func<Func<int?>> f) { return f()() ?? 0; }

    private static int Y(Func<Func<int>> f) { return f()(); }
    private static double Y(Func<Func<double>> f) { return f()(); }
}
