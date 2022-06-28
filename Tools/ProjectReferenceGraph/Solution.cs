using System.Text.RegularExpressions;

namespace ProjectReferenceGraph;

public partial class Solution
{
    public string FullPath { get; }

    public Solution(string path)
    {
        FullPath = Path.GetFullPath(path);
    }

    public string Directory => _directory ??= Path.GetDirectoryName(FullPath)!;
    private string? _directory;

    public IEnumerable<Project> Projects => _projects ??= GetProjects(this).ToArray();
    private Project[]? _projects;

    private static IEnumerable<Project> GetProjects(Solution sln, string extension = ".csproj")
    {
        var lines = File.ReadAllLines(sln.FullPath)
            .Where(l => l.StartsWith("Project") && l.Contains(extension));

        var reg = ProjectRegex();

        foreach (var line in lines)
        {
            var m = reg.Match(line);
            yield return new(sln, m.Groups["name"].Value, m.Groups["csproj"].Value);
        }
    }

    [RegexGenerator("""
        =\s+"(?<name>.*?)",\s+"(?<csproj>.*?\.csproj)"
        """)]
    public static partial Regex ProjectRegex();

    private Dictionary<string, Project>? _pathToProject;

    public Project? GetProjectFromFullPath(string projectFullPath)
    {
        _pathToProject ??= Projects.ToDictionary(p => p.FullPath);
        return _pathToProject.TryGetValue(projectFullPath, out var p) ? p : null;
    }
}
