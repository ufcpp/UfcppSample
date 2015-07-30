using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValueTuples
{
    using Sample;

    class Program
    {
        static void Main()
        {
            //WriteTuples();
            WriteSerializedData();
            //WriteSerializedData旧();
        }

        private static void WriteSerializedData()
        {
            WriteSerializedData(new PolyLine(new[] { new Sample.Point(1, 2), new Sample.Point(3, 4) }, new[] { 1.5, 1.41421356 }));
            WriteSerializedData(new Pair<Sample.Point, string>(new Sample.Point(10, 20), "abcd"));
            WriteSerializedData(new Sample.Point(10, 20));
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

        //private static void WriteSerializedData旧()
        //{
        //    WriteSerializedData旧(new Person(123, "abc def", "東京都千代田区千代田1-1"));
        //    WriteSerializedData旧(new Student(999, "あｗせｄｒｆｔｇｙ ふじこ", "千葉県浦安市舞浜1-1", 5, 2));
        //    WriteSerializedData旧(new Point(10, 20));
        //    WriteSerializedData旧(new Line(new Point(1, 2), new Point(3, 4)));
        //}

        //private static void WriteSerializedData旧(IRecord record)
        //{
        //    byte[] buffer;
        //    using (var s = new MemoryStream())
        //    using (var sw = new StreamWriter(s))
        //    {
        //        var serializer = new Serialization旧.MySerializer(sw);
        //        serializer.Serialize(record);
        //        sw.Flush();

        //        var len = s.Length;
        //        s.Seek(0, SeekOrigin.Begin);
        //        buffer = new byte[len];
        //        s.Read(buffer, 0, (int)len);
        //    }

        //    var serialized = Encoding.UTF8.GetString(buffer);
        //    Console.WriteLine(serialized);
        //}

        private static void WriteTuples()
        {
            Write(ValueTuple.Create(1, 2, 3, "abc", "def"));
            Console.WriteLine();

            Write(new Point(10, 20));
            Console.WriteLine();

            Write(new Person(123, "abc def", "東京都千代田区千代田1-1"));
            Console.WriteLine();

            var x = (ITuple)new Student(999, "あｗせｄｒｆｔｇｙ ふじこ", "千葉県浦安市舞浜1-1", 5, 2).Value;

            Write(x);
            Console.WriteLine();

            x[0] = 1;
            x[1] = "a";
            x[2] = "b";
            x[3] = 2;
            x[4] = 3;
            Write(x);
            Console.WriteLine();

            var line = new Line(new Point(1, 2), new Point(3, 4));
            var lineClone = line.DeepClone();

            line.A.X = 10;
            line.A.Y = 20;
            line.B.X = 30;
            line.B.Y = 40;

            Write(line);
            Write(lineClone);
        }

        private static void Write(IRecord record) => Write(record.Value);

        private static void Write(ITuple tuple)
        {
            foreach (var item in tuple.Values)
            {
                Console.WriteLine(item);
            }

            for (int i = 0; i < tuple.Count; i++)
            {
                Console.WriteLine(tuple[i]);
            }
        }
    }
}
