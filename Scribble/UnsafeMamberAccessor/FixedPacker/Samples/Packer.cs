using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

namespace FixedPacker.Samples
{
    class Packer
    {
        public static byte[] Pack(IReadOnlyList<DefinitionData.Sample> data)
        {
            using (var m = new MemoryStream())
            {
                Pack(data, m);
                return m.ToArray();
            }
        }

        public unsafe static void Pack(IReadOnlyList<DefinitionData.Sample> data, Stream stream)
        {
            var size = Unsafe.SizeOf<GeneratedData.Sample>();

            var buffer = stackalloc byte[size];
            var p = (GeneratedData.Sample*)(void*)buffer;

            using (var w = new BinaryWriter(stream))
            {
                w.Write((long)data.Count);

                var strIndex = 0;

                foreach (var x in data)
                {
                    p->Id = x.Id;
                    p->A = x.A;
                    p->B = x.B;
                    p->C = x.C;
                    p->DIndex = strIndex; strIndex += x.D.Length + 4;

                    for (int i = 0; i < size; i++)
                    {
                        var xx = buffer[i];
                        w.Write(xx);
                    }
                }

                foreach (var x in data)
                {
                    var b = x.D.Bytes.ToArray();
                    w.Write(b.Length);
                    w.Write(b);
                }
            }
        }
    }
}
