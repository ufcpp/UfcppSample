[module: System.Runtime.CompilerServices.NonNullTypes]

namespace System.Runtime.CompilerServices
{
    internal sealed class NonNullTypesAttribute : Attribute
    {
        public NonNullTypesAttribute(bool flag = true) { }
    }

    internal sealed class NullableAttribute : Attribute
    {
        public NullableAttribute() { }
        public NullableAttribute(bool[] x) { }
    }
}
