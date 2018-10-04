class EarlyReturn
{
    static string? NoWarning(object? x)
    {
        if (x == null) return null;
        return x.ToString();
    }

    static string? Warning(object? x)
    {
        if (x == null) return null;
        if (x is string s) return s; // add this line
        return x.ToString(); // CS8602
    }

    static string? NoWarningAgain(object? x)
    {
        if (x is string s) return s; // add this line
        if (x == null) return null;
        return x.ToString(); // no warning
    }
}
