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
            NewMethod("DeriveRestrictedAppContainerSidFromAppContainerSidAndRestrictedName");
            NewMethod("InternalFrameInternalFrameTitlePaneInternalFrameTitlePaneMaximizeButtonWindowNotFocusedState");
            NewMethod("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
        }

        private static void NewMethod(string s)
        {
            Console.WriteLine(s);

            var snake = s.CamelToSnake();
            Console.WriteLine(snake);

            var camel = snake.SnakeToCamel();
            Console.WriteLine(camel);

            Console.WriteLine(s == camel);
        }
    }
}
