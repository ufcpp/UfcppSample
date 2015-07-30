using System.IO;

namespace ValueTuples.Serialization
{
    public interface ISerializerFactory
    {
        ISerializer GetSerializer(Stream stream);
        IDeserializer GetDeserializer(Stream stream);
    }
}
