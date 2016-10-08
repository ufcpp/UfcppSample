using System;

namespace ConsoleApplication1.StringUtilities
{
    public struct CodePoint : IEquatable<CodePoint>
    {
        public CodePoint(uint value)
        {
            Value = value;
        }

        public uint Value { get; }

        public override string ToString() => Value.ToString("X");
        public override bool Equals(object obj) => obj is CodePoint x && Value == x.Value;
        public override int GetHashCode() => Value.GetHashCode();
        public bool Equals(CodePoint other) => Value == other.Value;
        public static bool operator ==(CodePoint x, CodePoint y) => x.Value == y.Value;
        public static bool operator !=(CodePoint x, CodePoint y) => x.Value != y.Value;
    }
}
