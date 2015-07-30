using System;
using System.Collections;
using System.IO;
using System.Linq;
using ValueTuples.Reflection;

namespace ValueTuples.Serialization
{
    public class MyFactory : ISerializerFactory
    {
        public IDeserializer GetDeserializer(Stream stream) => new MyDeserializer(new StreamReader(stream));

        public ISerializer GetSerializer(Stream stream) => new MySerializer(new StreamWriter(stream));
    }

    internal class MySerializer : ISerializer
    {
        StreamWriter _s;

        public MySerializer(StreamWriter s)
        {
            _s = s;
        }

        public void Dispose() => _s.Dispose();

        public void Serialize(object value)
        {
            if (value == null) return;

            var info = TypeRepository.Get(value.GetType());

            if(info.IsSimple)
            {
                _s.WriteLine(value.ToString());
                return;
            }

            if (info.IsArray)
            {
                var array = (IList)value;
                _s.WriteLine(array.Count);
                foreach (var item in array)
                {
                    Serialize(item);
                }
                return;
            }

            var accessor = info.GetAccessor(value);

            foreach (var f in info.Fields)
            {
                _s.WriteLine(f.Name);

                var x = accessor.Get(f.Index);
                Serialize(x);
            }
        }
    }

    internal class MyDeserializer : IDeserializer
    {
        StreamReader _s;

        public MyDeserializer(StreamReader s)
        {
            _s = s;
        }

        public void Dispose() => _s.Dispose();

        public object Deserialize(Type t)
        {
            if (t == typeof(int)) { return int.Parse(_s.ReadLine()); }
            if (t == typeof(string)) { return _s.ReadLine(); }

            var info = TypeRepository.Get(t);

            if(info.IsArray)
            {
                var length = int.Parse(_s.ReadLine());
                var array = info.GetArray(length);
                var elem = ((ArrayTypeInfo)info).ElementType.Type;

                for (int i = 0; i < length; i++)
                {
                    var item = Deserialize(elem);
                    array.SetValue(item, i);
                }
                return array;
            }

            var value = info.GetInstance();
            var accessor = info.GetAccessor(value);
            var count = accessor.Count;

            while (true)
            {
                var line = _s.ReadLine();
                if (line == null) break;

                var field = info.Fields.First(f => f.Name == line);
                var x = Deserialize(field.Type.Type);
                accessor.Set(field.Index, x);

                --count;
                if (count == 0) break;
            }

            return value;
        }
    }
}
