using System;
using System.IO;
using System.Text;

namespace ValueTuples
{
    using Sample;
    using System.Collections.Generic;
    using System.Linq;

    class Program
    {
        static void Main()
        {
            //PackUnpack();

            WriteSerializedData(new Serialization.MyFactory());
            Console.ReadKey();
            WriteSerializedData(new Serialization.MyBinaryFactory());
        }

        private static void PackUnpack()
        {
            var packed = Pack();

            foreach (var x in Unpack(packed))
            {
                if (x is MiniMessagePack.Number)
                {
                    var n = (MiniMessagePack.Number)x;
                    Console.WriteLine($"{n.Type}: {n}");
                }
                else
                {
                    Console.WriteLine($"{x.GetType().Name}: {x}");
                }
            }
        }

        private static IEnumerable<object> Unpack(byte[] packed)
        {
            var p = new MiniMessagePack.MiniMessagePacker();

            using (var s = new MemoryStream(packed))
            {
                for (int i = 0; i < 29; i++)
                {
                    yield return p.UnpackNumber(s);
                }

                yield return p.Unpack(s);
            }
        }

        private static byte[] Pack()
        {
            var p = new MiniMessagePack.MiniMessagePacker();

            using (var s = new MemoryStream())
            {
                p.Pack(s, (byte)1);
                p.Pack(s, (sbyte)1);
                p.Pack(s, (short)1);
                p.Pack(s, (ushort)1);
                p.Pack(s, (int)1);

                p.Pack(s, (uint)1);
                p.Pack(s, (long)1);
                p.Pack(s, (ulong)1);
                p.Pack(s, (float)1);
                p.Pack(s, (double)1);

                p.Pack(s, 0);
                p.Pack(s, 0xFF);
                p.Pack(s, 0xFFFF);
                p.Pack(s, 0xFFFFFFFF);
                p.Pack(s, 0xFFFFFFFFFFFFFFFF);

                p.Pack(s, -1);
                p.Pack(s, 0x7F);
                p.Pack(s, 0x7FFF);
                p.Pack(s, 0x7FFFFFFF);
                p.Pack(s, 0x7FFFFFFFFFFFFFFF);

                p.Pack(s, -10);
                p.Pack(s, -300);
                p.Pack(s, -70000);
                p.Pack(s, -3000000000);
                p.Pack(s, long.MinValue);

                p.Pack(s, 1e8);
                p.Pack(s, 1e15);
                p.Pack(s, true);
                p.Pack(s, false);

                p.Pack(s, "abc あいう αβγ 亜以宇");

                return s.ToArray();
            }
        }

        private static void WriteSerializedData(Serialization.ISerializerFactory factory)
        {
            WriteSerializedData(factory, new PolyLine(new[] { new Point(1, 2), new Point(3, 4) }, new[] { 1.5, 1.41421356 }));
            WriteSerializedData(factory, new Pair<Point, string>(new Point(10, 20), "abcd"));
            WriteSerializedData(factory, new Point(10, 20));
            WriteSerializedData(factory, new[] { 1, 2, 3, 4, 5 });
            WriteSerializedData(factory, new Unit[]
            {
                new Fighter(1, "Duran", 3.14),
                new Mage(2, "Angela", "Earthquake"),
                new Thief(3, "Hawk", 12, 15),
            });
        }

        private static byte[] Serialize(Serialization.ISerializerFactory factory, object value)
        {
            using (var s = new MemoryStream())
            {
                using (var serializer = factory.GetSerializer(s))
                    serializer.Serialize(value);

                return s.ToArray();
            }
        }

        private static object Deserialize(Serialization.ISerializerFactory factory, Type t, byte[] data)
        {
            using (var s = new MemoryStream(data))
            {
                using (var deserializer = factory.GetDeserializer(s))
                    return deserializer.Deserialize(t);
            }
        }

        private static void WriteSerializedData(Serialization.ISerializerFactory factory, object value)
        {
            var data = Serialize(factory, value);

            Console.WriteLine("length: " + data.Length);

            var serialized = Encoding.UTF8.GetString(data);
            Console.WriteLine(serialized);

            var deserialized = Deserialize(factory, value.GetType(), data);

            var e = deserialized as System.Collections.IEnumerable;
            if(e == null)
            {
                Console.WriteLine(deserialized);
            }
            else
            {
                Console.WriteLine(string.Join(", ", e.Cast<object>().Select(x => x.ToString())));
            }
        }
    }
}
