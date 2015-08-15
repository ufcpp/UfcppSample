namespace InterfaceSample
{
    class Program
    {
        static void Main(string[] args)
        {
            Bcl.IComparableSample.Main();
            Bcl.IEnumerableSample.Main();
            Bcl.IReadOnlyListSample.Main();
            Bcl.IDisposableSample.Main();
            Explicit.GenericInterfaceSample.Main();
            Explicit.ExpliciteImplementationSample.Main();
        }
    }
}
