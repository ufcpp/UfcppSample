using System;

namespace InterfaceSample.Explicit
{
    struct Id : IComparable<Id>, IEquatable<Id>
    {
        public int Value { get; set; }

        public int CompareTo(Id other) => Value.CompareTo(other.Value);

        public bool Equals(Id other) => Value == other.Value;
    }
}
