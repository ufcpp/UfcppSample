class Nullability
{
    public void Main()
    {
        int.Parse(null);
        AllowNull(NotNull());
        DisallowNull(NotNull());
        AllowNull(MaybeNull());
        DisallowNull(MaybeNull());
        AllowNull(FalseNotNull());
        DisallowNull(FalseNotNull());
    }

    string NotNull() => "";
    int DisallowNull(string s) => s.Length;

    string? MaybeNull() => null;
    int? AllowNull(string? s) => s?.Length;

    string FalseNotNull() => null;

    void M(string s)
    {
        int.Parse(s);

        string local = null;
        int.Parse(local);
    }
}
