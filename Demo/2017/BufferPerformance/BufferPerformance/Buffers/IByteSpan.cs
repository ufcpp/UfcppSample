namespace BufferPerformance.Buffers
{
    interface IByteSpan
    {
        ref byte this[int index] { get; }
        int Length { get; }
    }
}
