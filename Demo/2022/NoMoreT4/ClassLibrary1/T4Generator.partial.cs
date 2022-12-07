namespace ClassLibrary1;

public partial class T4Generator : IGenerator
{
    private readonly Type _type;
    public T4Generator(Type type) => _type = type;
    public static IGenerator Create(Type type) => new T4Generator(type);
}
