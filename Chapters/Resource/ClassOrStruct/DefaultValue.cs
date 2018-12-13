#pragma warning disable 649

namespace DefaultValue
{
    using System;

    struct Sample
    {
        public int I;
        public double D;
        public bool B;
        public string S;
    }

    public class Program
    {
        static Sample s;

        public static void Main(string[] args)
        {
            Console.WriteLine(s.I);
            Console.WriteLine(s.D);
            Console.WriteLine(s.B);
            Console.WriteLine(s.S);
        }
    }
}
