using SampleApp.Lib;
using System;
using System.Text.Utf8;
using System.Text.Json;
using SampleApp.Samples;

namespace SampleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var entryData = new Utf8String("{ \"Code\": 10, \"Name\": \"aiueoあいうえお亜以宇江男\", \"Id\": 1234605616436508552, \"Hash\": 173 }");
            var pointData = new Utf8String("{ \"X\": 1, \"Y\": 2, \"Z\": 3 }");

            var entry = new Entry();
            Deserialize1(entryData.Bytes, entry);
            Console.WriteLine(entry);

            entry.Value = default((int, string, long, byte));
            Deserialize2(entryData.Bytes, entry);
            Console.WriteLine(entry);

            var point = new Point();
            Deserialize1(pointData.Bytes, point);
            Console.WriteLine(point);

            point.Value = default((int, int, int));
            Deserialize2(pointData.Bytes, point);
            Console.WriteLine(point);

            const int Loops = 10000000;

            for (int n = 0; n < 2; n++)
            {
                using (Diagnostics.Measure.Start("entry pointer"))
                {
                    for (int i = 0; i < Loops; i++)
                        Deserialize1(entryData.Bytes, entry);
                }

                using (Diagnostics.Measure.Start("point pointer"))
                {
                    // アロケーション0！
                    for (int i = 0; i < Loops; i++)
                        Deserialize1(pointData.Bytes, point);
                }

                using (Diagnostics.Measure.Start("entry boxing"))
                {
                    for (int i = 0; i < Loops; i++)
                        Deserialize2(entryData.Bytes, entry);
                }

                using (Diagnostics.Measure.Start("point boxing"))
                {
                    for (int i = 0; i < Loops; i++)
                        Deserialize2(pointData.Bytes, point);
                }
            }
        }

        private static void Deserialize1<T>(ReadOnlySpan<byte> serialized, T item)
            where T: IMemberAccessor, INamedMemberAccessor
        {
            using (var reader = new JsonReader(new Utf8String(serialized)))
            {
                while (reader.Read())
                {
                    switch (reader.TokenType)
                    {
                        case JsonReader.JsonTokenType.ObjectStart:
                        case JsonReader.JsonTokenType.ObjectEnd:
                        case JsonReader.JsonTokenType.ArrayStart:
                        case JsonReader.JsonTokenType.ArrayEnd:
                            break;
                        case JsonReader.JsonTokenType.Property:
                            var name = reader.GetName();
                            item.Parse(item.GetIndex(name), reader.GetValue());
                            break;
                        case JsonReader.JsonTokenType.Value:
                            var value = reader.GetValue();
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private static void Deserialize2<T>(ReadOnlySpan<byte> serialized, T item)
            where T : IRecordAccessor, INamedMemberAccessor
        {
            using (var reader = new JsonReader(new Utf8String(serialized)))
            {
                while (reader.Read())
                {
                    switch (reader.TokenType)
                    {
                        case JsonReader.JsonTokenType.ObjectStart:
                        case JsonReader.JsonTokenType.ObjectEnd:
                        case JsonReader.JsonTokenType.ArrayStart:
                        case JsonReader.JsonTokenType.ArrayEnd:
                            break;
                        case JsonReader.JsonTokenType.Property:
                            var name = reader.GetName();
                            item.Parse(item.GetIndex(name), reader.GetValue());
                            break;
                        case JsonReader.JsonTokenType.Value:
                            var value = reader.GetValue();
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }

    static class Extensions
    {
        public static void Parse(this IRecordAccessor item, int index, Utf8String value)
        {
            item.Set(index, Parse(item.GetType(index), value));
        }

        public static void Parse(this IMemberAccessor item, int index, Utf8String value)
        {
            var p = item.GetPointer(index);
            p.Parse(value);
        }

        public static object Parse(Type t, Utf8String s)
        {
            if (t == typeof(string)) return s.ToString(); // ここの ToString が唯一のアロケーション
            else if (t == typeof(int))  return (int)ParseInt(s);
            else if (t == typeof(long)) return ParseInt(s);
            else if (t == typeof(byte)) return (byte)ParseInt(s);
            throw new NotSupportedException();
            //todo: 対応する型を増やす
        }

        public static void Parse(this TypedPointer p, Utf8String s)
        {
            if (p.Type == typeof(string)) p.Ref<string>() = s.ToString(); // ここの ToString が唯一のアロケーション
            else if (p.Type == typeof(int)) p.Ref<int>() = (int)ParseInt(s);
            else if (p.Type == typeof(long)) p.Ref<long>() = ParseInt(s);
            else if (p.Type == typeof(byte)) p.Ref<byte>() = (byte)ParseInt(s);
            //todo: 対応する型を増やす
        }

        private static long ParseInt(Utf8String s)
        {
            var v = 0L;
            foreach (var b in s.Bytes)
            {
                v *= 10;
                v += b - (byte)'0';
            }
            return v;
        }
    }
}
