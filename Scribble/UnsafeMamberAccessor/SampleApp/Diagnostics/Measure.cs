using System;
using System.Diagnostics;

namespace SampleApp.Diagnostics
{
    struct Measure : IDisposable
    {
        private readonly string _tag;
        private readonly long _beginMemory;
        private readonly Stopwatch _sw;
        private Measure(string tag)
        {
            _tag = tag;
            _sw = new Stopwatch();
            _sw.Start();
            _beginMemory = GC.GetTotalMemory(false);
        }

        public void Dispose()
        {
            var endMemory = GC.GetTotalMemory(false);
            _sw.Stop();
            Console.WriteLine($"{_tag}: {_sw.Elapsed}, {endMemory - _beginMemory}");
        }
        public static Measure Start(string tag) => new Measure(tag);
    }
}
