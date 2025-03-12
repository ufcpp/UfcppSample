using System.Buffers;

namespace ConsoleApp1.Formatter;

internal class ArrayFormatter<T> : IFormatter
{
    public Type Type => throw new NotImplementedException();

    public object Read(FormatterProvider provider, ref SequenceReader<byte> reader)
    {
        var length = (int)provider.GetFormatter(typeof(int)).Read(provider, ref reader);
        var t = provider.GetFormatter(typeof(T));
        var array = new T[length];
        for (int i = 0; i < length; i++)
        {
            array[i] = (T)t.Read(provider, ref reader);
        }
        return array;
    }

    public void Read(FormatterProvider provider, ref SequenceReader<byte> reader, scoped UnsafeRef value)
    {
        int length = 0;
        provider.GetFormatter(typeof(int)).Read(provider, ref reader, UnsafeRef.Create(ref length));

        if (value.As<object?>() is not T[] a || a.Length != length)
        {
            value.As<object?>() = new T[length];
        }

        var t = provider.GetFormatter(typeof(T));
        var array = value.As<T[]>();
        for (int i = 0; i < length; i++)
        {
            t.Read(provider, ref reader, UnsafeRef.Create(ref array[i]));
        }
    }

    public void Write(FormatterProvider provider, IBufferWriter<byte> writer, object value)
    {
        var array = (T[])value;
        provider.GetFormatter(typeof(int)).Write(provider, writer, array.Length);
        var t = provider.GetFormatter(typeof(T));
        for (int i = 0; i < array.Length; i++)
        {
            t.Write(provider, writer, array[i]);
        }
    }

    public void Write(FormatterProvider provider, IBufferWriter<byte> writer, scoped UnsafeRef value)
    {
        var array = value.As<T[]>();
        var length = array.Length;
        provider.GetFormatter(typeof(int)).Write(provider, writer, UnsafeRef.Create(ref length));
        var t = provider.GetFormatter(typeof(T));
        for (int i = 0; i < length; i++)
        {
            t.Write(provider, writer, UnsafeRef.Create(ref array[i]));
        }
    }
}
