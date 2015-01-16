using System.Collections.Generic;

namespace Inventories
{
    class IdComparer<T> : IEqualityComparer<T>
        where T : IIdentifiable
    {
        private IdComparer() { }

        public bool Equals(T x, T y)
        {
            if (ReferenceEquals(x, default(T)) && ReferenceEquals(y, default(T))) return true;
            if (ReferenceEquals(x, default(T)) || ReferenceEquals(y, default(T))) return false;
            return x.Id == y.Id;
        }

        public int GetHashCode(T obj)
        {
            return obj.GetHashCode();
        }

        public static readonly IdComparer<T> Singleton = new IdComparer<T>();
    }
}
