using System;

namespace ValueTuples.Serialization
{
    public interface IDeserializer : IDisposable
    {
        object Deserialize(Type t);
    }
}
