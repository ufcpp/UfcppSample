class Switch
{
    static void X(object? x)
    {
        switch (x)
        {
            case null:
                return;
            default:
                break;
        }

        object nonNull = x; // CS8600
    }
}
