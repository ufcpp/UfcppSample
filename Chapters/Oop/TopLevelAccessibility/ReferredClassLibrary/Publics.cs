public interface PublicInterface
{
    PublicDelegate X { get; }
}

public delegate void PublicDelegate();

namespace A
{
    public class PublicClass : PublicInterface, InternalInterface
    {
        private PublicStruct _value;

        public PublicDelegate X => _value.X;

        private InternalStruct _internal;

        InternalDelegate InternalInterface.X => _internal.X;
    }

    public struct PublicStruct
    {
        public PublicDelegate X { get; set; }
    }
}
