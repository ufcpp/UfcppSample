using System;

namespace ClassLibrary3
{
    public class Class1
    {
#if Shared
        public int X = 1;
#elif A
        public int X = 2;
#endif

#if NETSTANDARD2_0
        public int Y = 0;
#elif NET461
        public int Y = 1;
#elif NET35
        public int Y = 2;
#endif
    }
}
