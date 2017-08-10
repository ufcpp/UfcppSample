using System.Collections.Generic;
using static System.Linq.Expressions.Expression;

namespace PropertyAccessor.Accessors
{
    public static class EachCodeGenerator<T>
    {
        public delegate object Getter(ref T obj);
        public delegate void Setter(ref T obj, object value);

        public static IDictionary<string, Entry> Items { get; } = new Dictionary<string, Entry>(Generate());

        public struct Entry
        {
            public Getter Get { get; }
            public Setter Set { get; }

            public Entry(Getter getter, Setter setter)
            {
                Get = getter;
                Set = setter;
            }
        }

        private static IEnumerable<KeyValuePair<string, Entry>> Generate()
        {
            foreach (var p in typeof(T).GetProperties())
            {
                var obj = Parameter(typeof(T).MakeByRefType());

                var value = Parameter(typeof(object));
                var setex = Lambda<Setter>(
                    Assign(
                        MakeMemberAccess(obj, typeof(T).GetProperty(p.Name)),
                        Convert(value, p.PropertyType)),
                    obj, value);
                var setter = setex.Compile();

                var getex = Lambda<Getter>(
                    Convert(
                        MakeMemberAccess(obj, typeof(T).GetProperty(p.Name)),
                        typeof(object)),
                    obj);
                var getter = getex.Compile();

                yield return new KeyValuePair<string, Entry>(p.Name, new Entry(getter, setter));
            }
        }
    }
}
