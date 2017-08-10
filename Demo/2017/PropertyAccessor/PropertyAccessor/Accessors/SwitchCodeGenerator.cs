using System.Linq;
using static System.Linq.Expressions.Expression;

namespace PropertyAccessor.Accessors
{
    public static class SwitchCodeGenerator<T>
    {
        public delegate object Getter(ref T obj, string name);
        public delegate void Setter(ref T obj, string name, object value);

        public static Getter Get { get; } = GetGetter();
        public static Setter Set { get; } = GetSetter();

        private static Getter GetGetter()
        {
            var t = typeof(T);

            var properties = (
                from p in t.GetAllProperties()
                where p.GetGetMethod() != null
                select p
                ).ToArray();

            if (!properties.Any()) return (ref T obj, string name) => null;

            var instance = Parameter(t.MakeByRefType(), "instance");
            var propertyName = Parameter(typeof(string), "propertyName");

            var cases = (
                from p in properties
                select SwitchCase(
                    Convert(Property(instance, p), typeof(object)),
                    Constant(p.Name)
                    )
                ).ToArray();

            var ex = Lambda<Getter>(
                Switch(
                    propertyName,
                    Constant(null, typeof(object)),
                    cases.ToArray()),
                instance,
                propertyName
                );

            return ex.Compile();
        }

        private static Setter GetSetter()
        {
            var t = typeof(T);

            var properties = (
                from p in t.GetAllProperties()
                where p.GetSetMethod() != null
                select p
                ).ToArray();

            if (!properties.Any()) return (ref T obj, string name, object v) => { };

            var instance = Parameter(t.MakeByRefType(), "instance");
            var propertyName = Parameter(typeof(string), "propertyName");
            var value = Parameter(typeof(object), "value");

            //System.Linq.Expressions.Expression.
            var cases =
                from p in properties
                select SwitchCase(
                    Block(typeof(void), Assign(Property(instance, p), Convert(value, p.PropertyType))),
                    Constant(p.Name)
                    );

            var ex = Lambda<Setter>(
                Switch(
                    propertyName,
                    cases.ToArray()),
                instance,
                propertyName,
                value
                );

            return ex.Compile();
        }
    }
}
