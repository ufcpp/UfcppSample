using System;
using System.IO;

namespace ValueTuples
{
    public interface ISerializer
    {
        Stream Stream { get; set; }

        void Serialize(ITuple value, Func<int, string> getKey);

        void Deserialize(ITuple output, Func<string, int> getIndex);
    }
}
