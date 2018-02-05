using System;
using System.IO;
//using StringManipulation.Classic;
//using StringManipulation.Unsafe;
using StringManipulation.SafeStackalloc;

namespace StringManipulation
{
    class Program
    {
        static void Main(string[] args)
        {
            //var s = "DeriveRestrictedAppContainerSidFromAppContainerSidAndRestrictedName";
            var s = "InternalFrameInternalFrameTitlePaneInternalFrameTitlePaneMaximizeButtonWindowNotFocusedState";

            Console.WriteLine(s.ToInitialLower());
            Console.WriteLine(s.ToInitialUpper());
            Console.WriteLine(s.CamelToSnake());
            Console.WriteLine(s.CamelToSnake().SnakeToCamel());

            //foreach (var word in new Unsafe.UpperCaseSplitter(s))
            //{
            //    Console.WriteLine(word);
            //}

            //Console.WriteLine("----");

            //foreach (var word in new SafeStackalloc.UpperCaseSplitter(s))
            //{
            //    Console.WriteLine(new string(word));
            //}

            //File.OpenRead().Read()
        }
    }
}
