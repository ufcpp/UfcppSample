namespace System
{
    public delegate void Action<in T1>(T1 x1);
    public delegate void Action<in T1, in T2>(T1 x1, T2 x2);
    public delegate void Action<in T1, in T2, in T3>(T1 x1, T2 x2, T3 x3);
    public delegate void Action<in T1, in T2, in T3, in T4>(T1 x1, T2 x2, T3 x3, T4 x4);
}
