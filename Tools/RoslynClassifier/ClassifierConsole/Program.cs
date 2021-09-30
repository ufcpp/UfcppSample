using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis.Classification;

var set = new HashSet<string>();

MSBuildLocator.RegisterDefaults();

var path = "../../../../SampleProject/SampleProject.csproj";

using (var workspace = MSBuildWorkspace.Create())
{
    var project = await workspace.OpenProjectAsync(path);

    //foreach (var doc in project.Documents)
    //{
    //    Console.WriteLine(doc.Name);
    //}

    //return;

    foreach (var doc in project.Documents)
    {
        Console.WriteLine(doc.Name);

        var text = await doc.GetTextAsync();
        //var sem = await doc.GetSemanticModelAsync();
        //sem.Compilation.

        var classifiedSpans = await Classifier.GetClassifiedSpansAsync(doc, TextSpan.FromBounds(0, text.Length));

        foreach (var s in classifiedSpans)
        {
            set.Add(s.ClassificationType);
            Console.WriteLine($"{text.GetSubText(s.TextSpan)} ({s.TextSpan.Start}-{s.TextSpan.End}) : {s.ClassificationType}");
        }

        //Console.ReadLine();
    }
}

foreach (var type in set.OrderBy(x => x))
{
    Console.WriteLine(type);
}
