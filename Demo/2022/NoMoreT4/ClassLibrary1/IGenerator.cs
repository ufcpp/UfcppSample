namespace ClassLibrary1;

public interface IGenerator
{
    static abstract IGenerator Create(Type type);
    string TransformText();
}