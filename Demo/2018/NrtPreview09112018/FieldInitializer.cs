class FieldInitializer
{
    private static object A(object x) => x;

    private object M1() => null; // CS8603
    private object M2() => A(null); // CS8625

    private object X1 = null; // No warning
    private object X2 = A(null); // No warning
}
