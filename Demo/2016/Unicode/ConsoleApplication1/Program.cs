﻿namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            DualEncoding.Run();
            DecodeSample.Decode();
            Performance.Check();
            CompatibleWithBstr.WriteLayout();
            NoAllocation.AllocationCheck();
            AllCharactersInUnicodeData.Count().Wait();
            CharacterLength.WriteLength();
            ComparisonWithSystemString.Run();
        }
    }
}
