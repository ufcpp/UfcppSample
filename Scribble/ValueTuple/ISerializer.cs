using System;
using System.IO;

namespace ValueTuples
{
    public interface ISerializer
    {
        Stream Stream { get; set; }

        void Serialize(IRecord record);

        void Deserialize(IRecord record);
    }
}
