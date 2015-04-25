/// <summary>
/// Can be compiled in: C# 6
///
/// https://github.com/dotnet/roslyn/issues/2110
///
/// The C# specification had "invariant meaning" rule: https://msdn.microsoft.com/en-us/library/aa691351(v=vs.71).aspx
/// Roslyn has gotten rid of the "invariant meaning" rule.
/// </summary>
class InvariantMeaningInBlock
{
    double x;

    void F(bool b)
    {
        x = 1.0;
        if (b)
        {
            int x;
            x = 1;
        }
    }

#if false
    // FYI
    void F1(bool b)
    {
        if (b)
        {
            int x; // generates CS0136 still in C# 6.0.
            x = 1;
        }
        int x = 1.0;
    }
#endif

    static void Main()
    {
        var x = new InvariantMeaningInBlock();
        x.F(true);
        x.F(false);
    }
}

