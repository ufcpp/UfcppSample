namespace NullabilitySample
{
    public class NullError
    {
        static void M() => X(null);

        static int X(string s) => s.Length;
    }
}
