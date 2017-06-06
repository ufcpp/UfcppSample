using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using BufferPerformance.Buffers;
using System;

namespace BufferPerformance
{
    [MemoryDiagnoser]
    public class Program
    {
        const int Loop = 1000;
        const int Size = 500;
        const int DefaultCapacity = Loop * Size / 4;

        unsafe static void Main()
        {
            //Console.WriteLine(ReadWriteBuffer<BufferA, ArraySpan>(new BufferA(DefaultCapacity)));
            //Console.WriteLine(ReadWriteBuffer<BufferB, ArraySpan>(new BufferB(DefaultCapacity)));
            //Console.WriteLine(ReadWriteBuffer<BufferC, PointerSpan>(new BufferC(DefaultCapacity)));
            //Console.WriteLine(ReadWriteBuffer<BufferD, PointerSpan>(new BufferD(DefaultCapacity)));
            Console.WriteLine(ReadWriteBuffer<BufferE, PointerSpan>(new BufferE(DefaultCapacity)));

            //for (int i = 0; i < 1000; i++)
            //{
            //    ReadWriteBuffer<BufferB, ArraySpan>(new BufferB(DefaultCapacity));
            //}

            //for (int i = 0; i < 1000; i++)
            //{
            //    Console.WriteLine(XXX<BufferC, PointerSpan>(new BufferC(DefaultCapacity)));
            //    Console.WriteLine(GC.GetTotalMemory(true));
            //}

            //BenchmarkRunner.Run<Program>();
        }

        [Benchmark] public void A() => ReadWriteBuffer<BufferA, ArraySpan>(new BufferA(DefaultCapacity));
        [Benchmark] public void B() => ReadWriteBuffer<BufferB, ArraySpan>(new BufferB(DefaultCapacity));
        [Benchmark] public void C() => ReadWriteBuffer<BufferC, PointerSpan>(new BufferC(DefaultCapacity));
        [Benchmark] public void D() => ReadWriteBuffer<BufferD, PointerSpan>(new BufferD(DefaultCapacity));
        [Benchmark] public void E() => ReadWriteBuffer<BufferE, PointerSpan>(new BufferE(DefaultCapacity));

        static int ReadWriteBuffer<TBuffer, TSpan>(TBuffer buffer)
            where TBuffer : IBuffer<TSpan>
            where TSpan : IByteSpan
        {
            using (buffer)
            {
                for (int i = 0; i < Loop; i++)
                {
                    buffer.Reserve(Size);
                    var s = buffer.FreeSpan;
                    for (int j = 0; j < Size; j++)
                    {
                        s[j] = (byte)j;
                    }
                    buffer.Skip(Size);
                        Console.WriteLine("a " + i);
                }

                var w = buffer.WrittenSpan;
                var sum = 0;
                for (int j = 0; j < w.Length; j++)
                {
                    sum += w[j];
                }
                return sum;
            }
        }
    }
}
