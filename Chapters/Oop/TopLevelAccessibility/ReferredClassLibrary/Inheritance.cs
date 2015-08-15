namespace B
{
    public struct PublicStruct { }
    public interface PublicInterface { PublicStruct X { get; } }

    internal struct InternalStruct { }
    internal interface InternalInterface { InternalStruct X { get; } }

    public class PublicClass : PublicInterface, InternalInterface
    {
        public PublicStruct X => default(PublicStruct);
        InternalStruct InternalInterface.X => default(InternalStruct);
    }
}

namespace C
{
    internal class InternalClass { }

#if false
    // コンパイル エラー
    public class PublicClass : InternalClass
    {
    }
#endif
}
