using CsharpToHtml;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using System.IO;

namespace ClassifierWinApp.Models;

public class ClassfierWorkspace : IDisposable
{
    public string? CsprojPath { get; set; }

    private readonly MSBuildWorkspace _workspace;
    private Project? _project;

    public ClassfierWorkspace()
    {
        MSBuildLocator.RegisterDefaults();
        _workspace = MSBuildWorkspace.Create();
    }

    public bool IsOpen => _project is not null;

    public async ValueTask OpenProjectAsync()
    {
        if (CsprojPath is not { } proj) return;

        // 閉じて開きなおしでいい？
        // ソースコードの1行書き換えただけで close → 再 open するのちょっともったいない気がするものの。
        // Document 単位の reload 処理どこかにありそうなものの見つからず。
        if (_project is not null)
        {
            _workspace.CloseSolution();
        }
        _project = await _workspace.OpenProjectAsync(proj);

        var projectDirectory = Path.GetDirectoryName(Path.GetFullPath(proj)) ?? "";

        Documents = _project?.Documents?
            .Select(x => new ClassfierDocument(projectDirectory, x))
            .Where(x => !x.IsGenerated)
            ?? Array.Empty<ClassfierDocument>();
    }

    public IEnumerable<ClassfierDocument> Documents { get; private set; } = Array.Empty<ClassfierDocument>();

    public string? Text { get; private set; }

    public void Dispose()
    {
        _workspace.Dispose();
    }
}

public class ClassfierDocument
{
    private readonly string _projectDirectory;
    private readonly Document _document;

    public ClassfierDocument(string projectDirectory, Document document)
    {
        _projectDirectory = projectDirectory;
        _document = document;

        var path = document.FilePath;
        var name = path is null ? null : Path.GetRelativePath(_projectDirectory, path);

        ShortName = name;
        IsGenerated = string.IsNullOrEmpty(name)
            || name.StartsWith(@"obj\", StringComparison.Ordinal)
            || name.StartsWith(@"obj/", StringComparison.Ordinal);
    }

    public string? ShortName { get; }
    public bool IsGenerated { get; }

    public async Task<string> ClassifyAsync() => await _document.ToHtmlAsync(false);
}
