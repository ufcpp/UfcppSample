using System.Collections.Concurrent;

namespace ConsoleApp1.Formatter;

public class FormatterProvider
{
    private readonly ConcurrentDictionary<Type, IFormatter?> _formatters = [];

    public IFormatter? GetFormatter(Type t)
    {
        return _formatters.GetOrAdd(t, Create);
    }

    private IFormatter? Create(Type type)
    {
        if (type.IsArray)
        {
            var elementType = type.GetElementType();
            var formatter = (IFormatter)Activator.CreateInstance(typeof(ArrayFormatter<>).MakeGenericType(elementType));
            return formatter;
        }

        return Primitives.GetFormatter(type);
    }

    public void Add(IFormatter formatter)
    {
        _formatters[formatter.Type] = formatter;
    }
}
