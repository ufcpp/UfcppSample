namespace IdentifierScope.Loop
{
    using System;
    using System.Linq;

    public class Sample
    {
        public static void M()
        {
            for (int i = 0; i < 5; i++)
            {
                // for の i のスコープはこのブロック内
                Console.WriteLine(i);
            }

            foreach (var i in Enumerable.Range(0, 5))
            {
                // foreach の i のスコープはこのブロック内
                // for の方の i とは別物
                Console.WriteLine(i);
            }
        }
    }
}
