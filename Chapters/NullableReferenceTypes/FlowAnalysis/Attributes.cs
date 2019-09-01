using System;
using System.Collections.Generic;
using System.IO;

namespace FlowAnalysis.Attributes
{
    class Program
    {
        static void Main()
        {
            //AllowNullAttribute("");
            //DisallowNullAttribute();
            //MaybeNullAttribute();
            NotNullAttribute();
        }

        private static void AllowNullAttribute(string path)
        {
            var t = new StreamWriter(path);
            Console.WriteLine(t.NewLine.Length);
            t.NewLine = null;
        }

        private static void DisallowNullAttribute()
        {
            var c = EqualityComparer<string>.Default;
            c.Equals("", null);
            var h = c.GetHashCode(null);
        }

        private static void MaybeNullAttribute()
        {
            var array = new[] { "a", "bc" };
            var s = Array.Find(array, s => s.Length == 3);
            Console.WriteLine(s == null);
        }

        private static void NotNullAttribute()
        {
            int[]? array = null;
            Array.Resize(ref array, 1);
            Console.WriteLine(array.Length);
        }
    }
}
