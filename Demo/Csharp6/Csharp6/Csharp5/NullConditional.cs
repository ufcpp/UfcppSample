using System;

namespace Csharp6.Csharp5
{
    class NullConditionalSample
    {
        public string Name { get; set; }

        public static int? X(NullConditionalSample s)
        {
            if (s == null) return null;
            var name = s.Name;
            if (name == null) return null;
            return name.Length;
        }

        static char? X(string s, int i)
        {
            if (s == null) return null;
            return s[i];
        }

        static T Y<T>(Func<T> f)
            where T : class
        {
            if (f == null) return null;
            return f.Invoke();
        }
    }
}
