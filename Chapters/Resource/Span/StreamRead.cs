namespace Span.StreamRead
{
    using System;
    using System.IO;

    class Program
    {
        static void Main()
        {
            //GenerateTestData();

            ArrayRead();
        }

        private static void GenerateTestData()
        {
            using (var f = new BinaryWriter(File.OpenWrite("test.data")))
            {
                for (int i = 0; i < 10000; i++)
                {
                    f.Write((byte)i);
                }
            }
        }

        private static void ArrayRead()
        {
            const int BufferSize = 128;

            using (var f = File.OpenRead("test.data"))
            {
                var rest = (int)f.Length;
                var buffer = new byte[BufferSize];

                while (true)
                {
                    var read = f.Read(buffer, 0, Math.Min(rest, BufferSize));
                    rest -= read;
                    if (rest == 0) break;

                    // buffer に対して何か処理する
                    for (int i = 0; i < read; i++)
                    {
                        Console.Write(buffer[i].ToString("X"));
                    }
                    Console.WriteLine();
                }
            }
        }

        private static void SpanRead()
        {
            const int BufferSize = 128;

            using (var f = File.OpenRead("test.data"))
            {
                var rest = (int)f.Length;
                // Span<byte> で受け取ることで、new (配列)を stackalloc (スタック確保)に変更できる
                Span<byte> buffer = stackalloc byte[BufferSize];

                while (true)
                {
                    // Read(Span<byte>) が追加された
                    var read = f.Read(buffer);
                    rest -= read;
                    if (rest == 0) break;

                    // buffer に対して何か処理する
                    for (int i = 0; i < read; i++)
                    {
                        Console.Write(buffer[i].ToString("X"));
                    }
                    Console.WriteLine();
                }
            }
        }
    }
}
