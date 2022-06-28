using System.Text.RegularExpressions;

namespace ProjectReferenceGraph;

public partial class Project
{
    public Solution Solution { get; }
    public string Name { get; }
    public string FullPath { get; }

    public Project(Solution solution, string name, string projectRelativePath)
    {
        Solution = solution;
        Name = name;
        FullPath = Path.GetFullPath(projectRelativePath, solution.Directory);
    }

    public string Directory => _directory ??= Path.GetDirectoryName(FullPath)!;
    private string? _directory;

    public IEnumerable<Project> ProjectReferences => _projectReferences ??= GetProjectReferences().ToArray();
    private Project[]? _projectReferences;

    private IEnumerable<Project> GetProjectReferences()
    {
        foreach (var projPath in GetProjectReferences(FullPath))
        {
            if (Solution.GetProjectFromFullPath(projPath) is { } p)
                yield return p;
        }
    }

    private static IEnumerable<string> GetProjectReferences(string fullPath)
    {
        var directory = Path.GetDirectoryName(fullPath)!;

        var lines = File.ReadAllLines(fullPath);

        // ProjectReference
        var reg = ProjectReferenceRegex();
        foreach (var line in lines)
        {
            var m = reg.Match(line);
            if (!m.Success) continue;

            var relativePath = m.Groups["path"].Value;
            var refFullPath = Path.GetFullPath(relativePath, directory);
            yield return refFullPath;
        }

        // Import
        reg = ImportRegex();
        foreach (var line in lines)
        {
            var m = reg.Match(line);
            if (!m.Success) continue;

            var relativePath = m.Groups["path"].Value;
            var importFullPath = Path.GetFullPath(relativePath, directory);

            foreach (var sub in GetProjectReferences(importFullPath))
            {
                yield return sub;
            }
        }
    }

    [RegexGenerator("""
        ProjectReference\s+Include="(?<path>.*?)"
        """)]
    public static partial Regex ProjectReferenceRegex();

    [RegexGenerator("""
        Import\s+Project="(?<path>.*?)"
        """)]
    public static partial Regex ImportRegex();
}
