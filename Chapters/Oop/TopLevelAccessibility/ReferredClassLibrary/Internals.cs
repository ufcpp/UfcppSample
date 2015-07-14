internal interface InternalInterface
{
    InternalDelegate X { get; }
}

internal delegate void InternalDelegate();

namespace A
{
    internal class InternalClass : InternalInterface
    {
        private InternalStruct _value;

        public InternalDelegate X => _value.X;
    }

    internal struct InternalStruct
    {
        public InternalDelegate X { get; set; }
    }
}
