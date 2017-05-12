using System;

namespace ClassLibraryStd20
{
    public class Class1
    {
#if NET35
        public static string Name => ".NET Framework 3.5";
#elif NETSTANDARD1_0
        public static string Name => ".NET Standard 1.0";
#endif
    }
}
