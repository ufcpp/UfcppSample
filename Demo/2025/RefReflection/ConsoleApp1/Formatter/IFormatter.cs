using System.Buffers;

namespace ConsoleApp1.Formatter;

public interface IFormatter
{
    Type Type { get; }

    // via object
    void Write(FormatterProvider provider, IBufferWriter<byte> writer, object value);
    object Read(FormatterProvider provider, ref SequenceReader<byte> reader);

    // via UnsafeRef
    void Write(FormatterProvider provider, IBufferWriter<byte> writer, scoped UnsafeRef value);
    void Read(FormatterProvider provider, ref SequenceReader<byte> reader, scoped UnsafeRef value);
}
