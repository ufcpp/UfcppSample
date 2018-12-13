namespace SyntaxHighlighter
{
    public class Mode
    {
        public string Name { get; }
        public string Tag { get; }
        private Mode(string name, string tag) => (Name, Tag) = (name, tag);

        public static readonly Mode Csharp = new Mode("C#", "source");
        public static readonly Mode Xml = new Mode("XML", "xsource");
        public static readonly Mode Asm = new Mode("Asm", "source");
    }
}
