namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            DecodeSample.Decode();
            Performance.Check();
            return;
            CompatibleWithBstr.WriteLayout();
            NoAllocation.AllocationCheck();
            AllCharactersInUnicodeData.Count().Wait();
            CharacterLength.WriteLength();
        }
    }
}
