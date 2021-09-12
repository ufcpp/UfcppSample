m(null);

static void m(string? myArgument)
{
    ArgumentNullException.ThrowIfNull(myArgument);
}
