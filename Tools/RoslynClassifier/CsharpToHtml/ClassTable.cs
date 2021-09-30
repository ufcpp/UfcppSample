namespace CsharpToHtml;

public class ClassTable
{
    public static string? ClassToColor(string? @class) => @class switch
    {
        "keyword" => "0000FF",
        "control" => "8F08C4",
        "method" => "74531F",
        "type" => "2B91AF",
        "string" => "A31515",
        "variable" => "1F377F",
        "comment" => "008000",
        "excluded" => "686868",
        "preprocess" => "686868",
        _ => null,
    };

    public static string? TypeToClass(string classificationType) => classificationType switch
    {
        "keyword" => "keyword",

        "keyword - control" => "control",

        "method name" => "method",

        "class name"
        or "struct name"
        or "enum name"
        or "interface name"
        or "record class name"
        or "record struct name"
        or "type parameter name"
        => "type",

        "string" or "string - verbatim" => "string",

        "local name" or "parameter name" => "variable",

        "comment"
        or "xml doc comment - attribute name"
        or "xml doc comment - attribute quotes"
        or "xml doc comment - delimiter"
        or "xml doc comment - name"
        or "xml doc comment - text"
        => "comment",

        "excluded code" => "excluded",
        "preprocessor keyword" => "preprocess",

        "identifier" => null,
        "namespace name" => null,
        "number" => null,
        "operator" => null,
        "property name" => null,
        "punctuation" => null,

        _ => null,
    };
}
