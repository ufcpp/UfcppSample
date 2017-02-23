using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Jobs;
using BenchmarkDotNet.Engines;
using System;

namespace Generics
{
    [SimpleJob(RunStrategy.Throughput)]
    public class BenchmarkCode
    {
        [Benchmark]
        public static void GenericCInt() => Target.Max<CInt>(Target.Max<CInt>(1, 3), 2);
        [Benchmark]
        public static void InstantiatedCInt() => Instantiated.Max(Instantiated.Max((CInt)1, (CInt)3), (CInt)2);

        [Benchmark]
        public static void GenericCString() => Target.Max<CString>(Target.Max<CString>("abc", "acd"), "aab");
        [Benchmark]
        public static void InstantiatedCString() => Instantiated.Max(Instantiated.Max((CString)"abc", (CString)"acd"), (CString)"aab");

        [Benchmark]
        public static void GenericSInt() => Target.Max<SInt>(Target.Max<SInt>(1, 3), 2);
        [Benchmark]
        public static void InstantiatedSInt() => Instantiated.MaxSInt(Instantiated.MaxSInt((SInt)1, (SInt)3), (SInt)2);
        [Benchmark]
        public static void ObjectSInt() => Instantiated.Max(Instantiated.Max((SInt)1, (SInt)3), (SInt)2);

        [Benchmark]
        public static void GenericSString() => Target.Max<SString>(Target.Max<SString>("abc", "acd"), "aab");
        [Benchmark]
        public static void InstantiatedSString() => Instantiated.MaxSString(Instantiated.MaxSString((SString)"abc", (SString)"acd"), (SString)"aab");
        [Benchmark]
        public static void ObjectSString() => Instantiated.Max(Instantiated.Max((SString)"abc", (SString)"acd"), (SString)"aab");
    }
}
