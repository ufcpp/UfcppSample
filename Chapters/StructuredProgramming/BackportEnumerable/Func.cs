namespace System
{
    public delegate TResult Func<in T1, out TResult>(T1 x1);
    public delegate TResult Func<in T1, in T2, out TResult>(T1 x1, T2 x2);
    public delegate TResult Func<in T1, in T2, in T3, out TResult>(T1 x1, T2 x2, T3 x3);
    public delegate TResult Func<in T1, in T2, in T3, in T4, out TResult>(T1 x1, T2 x2, T3 x3, T4 x4);
}
