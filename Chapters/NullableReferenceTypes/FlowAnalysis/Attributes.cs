using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Xml.Schema;

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

        private static void MaybeNullWhenAttribute(Dictionary<int, string> map)
        {
            if (map.TryGetValue(1, out var s)) Console.WriteLine(s.Length);
            else Console.WriteLine(s.Length);
        }

        private static void NotNullWhenAttribute(string? s)
        {
            if (string.IsNullOrEmpty(s)) Console.WriteLine(s.Length);
            else Console.WriteLine(s.Length);
        }

        private static void NotNullIfNotNullAttribute()
        {
            var l1 = Path.GetFileName("sample.txt").Length;
            var l2 = Path.GetFileName(null).Length;
        }

        private static void DoesNotReturnAttribute(bool flag)
        {
            string? s = null;
            if (flag) s = "abc";
            else Environment.FailFast("fail");
            Console.WriteLine(s.Length);
        }

        private static void DoesNotReturnIfAttribute(string? s)
        {
            Debug.Assert(s != null);
            Console.WriteLine(s.Length);
        }
    }
}
