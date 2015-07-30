using System;
using System.IO;
using System.Text;

namespace ValueTuples
{
    using Sample;

    class Program
    {
        static void Main()
        {
            WriteSerializedData();
        }

        private static void WriteSerializedData()
        {
            WriteSerializedData(new PolyLine(new[] { new Point(1, 2), new Point(3, 4) }, new[] { 1.5, 1.41421356 }));
            WriteSerializedData(new Pair<Point, string>(new Point(10, 20), "abcd"));
            WriteSerializedData(new Point(10, 20));
            WriteSerializedData(new[] { 1, 2, 3, 4, 5 });
            WriteSerializedData(new Unit[]
            {
                new Fighter(1, "Duran", 3.14),
                new Mage(2, "Angela", "Earthquake"),
                new Thief(3, "Hawk", 12, 15),
            });
        }

        private static Serialization.ISerializerFactory _factory = new Serialization.MyFactory();

        private static byte[] Serialize(object value)
        {
            using (var s = new MemoryStream())
            {
                using (var serializer = _factory.GetSerializer(s))
                    serializer.Serialize(value);

                return s.ToArray();
            }
        }

        private static object Deserialize(Type t, byte[] data)
        {
            using (var s = new MemoryStream(data))
            {
                using (var deserializer = _factory.GetDeserializer(s))
                    return deserializer.Deserialize(t);
            }
        }

        private static void WriteSerializedData(object value)
        {
            var data = Serialize(value);

            var serialized = Encoding.UTF8.GetString(data);
            Console.WriteLine(serialized);

            var deserialized = Deserialize(value.GetType(), data);
            Console.WriteLine(deserialized);
        }
    }
}
