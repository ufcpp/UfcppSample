using System.Buffers;

namespace ConsoleApp1.Formatter;

public class RecordFormatter(Type type, RecordMemberInfo[] members) : IFormatter
{
    public Type Type => type;

    public object Read(FormatterProvider provider, ref SequenceReader<byte> reader)
    {
        var value = Activator.CreateInstance(Type);
        foreach (var m in members)
        {
            m.Setter(value, provider.GetFormatter(m.Type).Read(provider, ref reader));
        }
        return value;
    }

    public void Read(FormatterProvider provider, ref SequenceReader<byte> reader, scoped UnsafeRef value)
    {
        if (!type.IsValueType && value.As<object?>() == null)
        {
            value.As<object?>() = Activator.CreateInstance(Type);
        }

        foreach (var m in members)
        {
            provider.GetFormatter(m.Type).Read(provider, ref reader, m.RefGetter(value));
        }
    }

    public void Write(FormatterProvider provider, IBufferWriter<byte> writer, object value)
    {
        foreach (var m in members)
        {
            provider.GetFormatter(m.Type).Write(provider, writer, m.Getter(value));
        }
    }

    public void Write(FormatterProvider provider, IBufferWriter<byte> writer, scoped UnsafeRef value)
    {
        foreach (var m in members)
        {
            provider.GetFormatter(m.Type).Write(provider, writer, m.RefGetter(value));
        }
    }
}

public readonly struct RecordMemberInfo(Type type, Func<object, object> getter, Action<object, object> setter, Func<UnsafeRef, UnsafeRef> refGetter)
{
    public Type Type { get; } = type;

    // via object
    public Func<object, object> Getter { get; } = getter;
    public Action<object, object> Setter { get; } = setter;

    // via UnsafeRef
    public Func<UnsafeRef, UnsafeRef> RefGetter { get; } = refGetter;
}
