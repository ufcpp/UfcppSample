using System.Text;

namespace ClassLibrary1;

public partial class InterpolationGenerator : IGenerator
{
    private readonly Type _type;
    public InterpolationGenerator(Type type) => _type = type;
    public static IGenerator Create(Type type) => new InterpolationGenerator(type);

    public string TransformText()
    {
        var s = new StringBuilder();

        s.Append($$"""
namespace {{_type.Namespace}};

public partial static class {{_type.Name}}Extensions
{

""");
        foreach (var f in _type.GetFields())
        {
            s.Append($$"""
    public void Set{{f.Name}}(this {{_type.Name}} x, object value) => x.{{f.Name}} = ({{f.FieldType.FullName}})value;

""");
        }
        s.Append($$"""
}

""");

        return s.ToString();
    }
}
