using System.Collections.Generic;

namespace BitOperations
{
    public interface IBits : IEnumerable<bool>, IReadOnlyList<bool>
    {
        new bool this[int index] { get; set; }
    }
}
