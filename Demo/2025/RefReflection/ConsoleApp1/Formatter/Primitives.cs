using System.Buffers;
using System.Collections.Frozen;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ConsoleApp1.Formatter;

public class Primitives
{
    private static readonly FrozenDictionary<Type, IFormatter> _formatters = new IFormatter[]
    {
        new LittleEndianFormatter<byte>(),
        new LittleEndianFormatter<sbyte>(),
        new LittleEndianFormatter<short>(),
        new LittleEndianFormatter<ushort>(),
        new LittleEndianFormatter<int>(),
        new LittleEndianFormatter<uint>(),
        new LittleEndianFormatter<long>(),
        new LittleEndianFormatter<ulong>(),
        new LittleEndianFormatter<float>(),
        new LittleEndianFormatter<double>(),
    }.ToFrozenDictionary(x => x.Type);

    public static IFormatter? GetFormatter(Type t) => _formatters.TryGetValue(t, out var f) ? f : null;
}

internal class LittleEndianFormatter<T> : IFormatter
    where T : unmanaged
{
    public Type Type => typeof(T);

    private static T Cast(ReadOnlySpan<byte> span) => MemoryMarshal.Cast<byte, T>(span)[0];
    private static ReadOnlySpan<byte> Cast(ref T r)
    {
        var span = MemoryMarshal.CreateReadOnlySpan(ref r, 1);
        return MemoryMarshal.Cast<T, byte>(span);
    }

    public object Read(FormatterProvider provider, ref SequenceReader<byte> reader)
    {
        var size = Unsafe.SizeOf<T>();
        if (reader.UnreadSpan.Length >= size)
        {
            var span = reader.UnreadSpan[..size];
            var value = Cast(span);
            reader.Advance(size);
            return value;
        }
        else
        {
            var span = (stackalloc byte[size]);
            reader.UnreadSequence.CopyTo(span);
            var value = Cast(span);
            reader.Advance(size);
            return value;
        }
    }

    public void Read(FormatterProvider provider, ref SequenceReader<byte> reader, scoped UnsafeRef value)
    {
        var size = Unsafe.SizeOf<T>();
        if (reader.UnreadSpan.Length >= size)
        {
            var span = reader.UnreadSpan[..size];
            value.As<T>() = Cast(span);
            reader.Advance(size);
        }
        else
        {
            var span = (stackalloc byte[size]);
            reader.UnreadSequence.CopyTo(span);
            value.As<T>() = Cast(span);
            reader.Advance(size);
        }
    }

    public void Write(FormatterProvider provider, IBufferWriter<byte> writer, object value)
    {
        writer.Write(Cast(ref Unsafe.Unbox<T>(value)));
    }

    public void Write(FormatterProvider provider, IBufferWriter<byte> writer, scoped UnsafeRef value)
    {
        writer.Write(Cast(ref value.As<T>()));
    }
}
