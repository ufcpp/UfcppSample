namespace BitOperations
{
    /// <summary>
    /// Type Class for reading/writing n-th bit and shifting.
    /// </summary>
    public interface SBitOperator<T>
    {
        int Size { get; }
        bool GetBit(ref T x, int index);
        void SetBit(ref T x, int index, bool value);
        T RightShift(T x);
    }
}
