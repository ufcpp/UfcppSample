using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

namespace FixedPacker
{
    public interface IPacker<TDefinition>
    {
        void Pack(IReadOnlyList<TDefinition> data, Stream stream);
    }

    public static class PackerExtensions
    {
        public static byte[] Pack<TDefinition>(this IPacker<TDefinition> packer, IReadOnlyList<TDefinition> data)
        {
            using (var m = new MemoryStream())
            {
                packer.Pack(data, m);
                return m.ToArray();
            }
        }
    }

    public struct Packer<TDefinition, TGenerated> : IPacker<TDefinition>
    {
        public delegate void NewFunction(TDefinition d, Func<int, int> getIndex, ref TGenerated g);
        public delegate IEnumerable<ReadOnlySpan<byte>> GetBinariesFunction(TDefinition d);

        private NewFunction _new;
        private GetBinariesFunction _getBinaries;

        public Packer(NewFunction @new, GetBinariesFunction getBinaries)
        {
            _new = @new;
            _getBinaries = getBinaries;
        }

        public unsafe void Pack(IReadOnlyList<TDefinition> data, Stream stream)
        {
            var size = Unsafe.SizeOf<TGenerated>();

            var buffer = stackalloc byte[size];
            ref var p = ref Unsafe.AsRef<TGenerated>((void*)buffer);

            using (var w = new BinaryWriter(stream))
            {
                w.Write((long)data.Count);

                var binaryIndex = 0;

                foreach (var d in data)
                {
                    _new(d, len => { var i = binaryIndex; binaryIndex += len + 4; return i; }, ref p);

                    for (int i = 0; i < size; i++)
                    {
                        var xx = buffer[i];
                        w.Write(xx);
                    }
                }

                if (binaryIndex != 0)
                {
                    foreach (var d in data)
                    {
                        foreach (var b in _getBinaries(d))
                        {
                            w.Write(b.Length);
                            w.Write(b.ToArray());
                        }
                    }
                }
            }
        }
    }
}
