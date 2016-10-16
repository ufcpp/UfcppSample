namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            string s = "absadraew";

            {
                var begin = System.GC.GetTotalMemory(false);
                for (int i = 0; i < 10000; i++)
                {
                    foreach (var c in s)
                    {
                    }
                }
                var end = System.GC.GetTotalMemory(false);
                System.Console.WriteLine(end - begin);
            }
            return;


            DecodeSample.Decode();
            Performance.Check();
            CompatibleWithBstr.WriteLayout();
            NoAllocation.AllocationCheck();
            AllCharactersInUnicodeData.Count().Wait();
            CharacterLength.WriteLength();
        }
    }
}
