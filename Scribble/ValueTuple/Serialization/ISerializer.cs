using System;

namespace ValueTuples.Serialization
{
    public interface ISerializer : IDisposable
    {
        void Serialize(object value);
    }
}
