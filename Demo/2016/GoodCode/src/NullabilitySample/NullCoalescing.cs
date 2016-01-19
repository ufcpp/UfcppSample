namespace NullabilitySample
{
    public class NullCoalescing
    {
        static void Run()
        {
            int? n = null;
            int x = n ?? 0; // null だったら代わりに0を使う
        }
    }
}
