using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis.Classification;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.CodeAnalysis.Text;

MSBuildLocator.RegisterDefaults();

// MSBuildWorkspace はデフォルトで MEF から LanguageService を読むようになってるらしい。
using var workspace = MSBuildWorkspace.Create();

// Example.csproj を開いて、Program.cs の中身を読む。
var project = await workspace.OpenProjectAsync("../../../../Example/Example.csproj");
var doc = project.Documents.First(d => d.Name == "Program.cs");
var text = await doc.GetTextAsync();

// Program.cs の構文解析結果を表示。
var classifiedSpans = await Classifier.GetClassifiedSpansAsync(doc, TextSpan.FromBounds(0, text.Length));
foreach (var s in classifiedSpans)
{
    Console.WriteLine($"{text.GetSubText(s.TextSpan)} ({s.TextSpan.Start}-{s.TextSpan.End}) : {s.ClassificationType}");
}
