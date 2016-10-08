namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            NoAllocation.Test();
            AllCharactersInUnicodeData.Count().Wait();
            CharacterLength.WriteLength();
        }
    }
}
