#if Ver4

using System;

namespace VersionSample.Csharp4
{
    /// <summary>
    /// dynamic はガッツリと System.Core(v4.0以上)、Microsoft.Csharp アセンブリ(.NET 4で導入)に依存。
    /// これも、おそらくは同名・同機能のクラスをいくつか自作すれば .NET 3.0 以前で使えるものの、簡単に自作できるようなクラスじゃない。
    /// C# の機能の中で、同時期より前の .NET 上で動かせない数少ない機能。
    /// </summary>
    class DynamicSample
    {
        public void X()
        {
            var x1 = new Csharp3.Point(10, 20);
            var x2 = new { X = 10, Y = 20 };
            var x3 = new Vector3 { X = 1.5f, Y = 2.1f, Z = 3.3f };
            var x4 = new Record { X = "one", Y = "two" };

            DynamicX(x1);
            DynamicX(x2);
            DynamicX(x3);
            DynamicX(x4);
        }

        public void DynamicX(dynamic x)
        {
            Console.WriteLine(x.X + x.Y);
        }

        private class Vector3
        {
            public float X { get; set; }
            public float Y { get; set; }
            public float Z { get; set; }
        }

        private class Record
        {
            public string X { get; set; }
            public string Y { get; set; }
        }
    }
}

#endif