using ProjectReferenceGraph;

var slnPath = args[0];

writeMermaid(new(slnPath));

static void writeMermaid(Solution sln)
{
    Console.WriteLine("""
        ```mermaid
        graph LR

        """);

    foreach (var p in sln.Projects)
    {
        foreach (var r in p.ProjectReferences)
        {
            Console.WriteLine($"{p.Name} --> {r.Name}");
        }
    }

    Console.WriteLine("""
        ```
        """);
}
