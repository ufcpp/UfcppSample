namespace FileGenerator;

[ScribanSourceGeneretor.ClassMember("""
    {{
    for $t in ["bool","byte","int","double"]
    ~}}
        public static bool TryParse(this string s, out {{$t}} x) => {{$t}}.TryParse(s, out x);
    {{ end }}
    """)]
internal static partial class Extensions
{
}
