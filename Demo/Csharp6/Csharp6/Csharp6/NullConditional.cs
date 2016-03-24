using System;

namespace Csharp6.Csharp6
{
    class NullConditionalSample
    {
        public string Name { get; set; }

        public static int? X(NullConditionalSample s) => s?.Name?.Length;
        static char? X(string s, int i) => s?[i];
        static T Y<T>(Func<T> f)
            where T : class
            => f?.Invoke();
    }
}
