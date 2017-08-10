using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static System.Reflection.BindingFlags;

namespace PropertyAccessor
{
    static class Extensions
    {
        public static IEnumerable<PropertyInfo> GetAllProperties(this Type t) => t.GetProperties(Public | Instance).Where(p => !p.GetCustomAttributes().Any(a => a.GetType().Name.Contains("Ignore")));
        public static void Deconstruct<TKey, TValue>(this KeyValuePair<TKey, TValue> p, out TKey key, out TValue value) => (key, value) = (p.Key, p.Value);
    }
}
