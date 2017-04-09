using BenchmarkDotNet.Attributes;
using System;

namespace TupleMutableStruct.MemoryLayout
{
    public class VectorPerformance
    {
        [Setup]
        public void Setup()
        {
            data = new(byte a, byte b, byte c, byte d, byte e, byte f, byte g, byte h)[N];

            var r = new Random();
            for (int i = 0; i < N; i++)
            {
                data[i] = r.RandomVector();
            }
        }

        const int N = 2000;
        (byte a, byte b, byte c, byte d, byte e, byte f, byte g, byte h)[] data;

        [Benchmark]
        public SafeAdd.Vector XVector()
        {
            SafeAdd.Vector x = data[0];
            for (int i = 1; i < N; i++)
            {
                x = x.A * (x + data[i]);
            }
            return x;
        }

        [Benchmark]
        public PointerAdd.Vector YVector()
        {
            PointerAdd.Vector x = data[0];
            for (int i = 1; i < N; i++)
            {
                x = x.A * (x + data[i]);
            }
            return x;
        }
    }
}
