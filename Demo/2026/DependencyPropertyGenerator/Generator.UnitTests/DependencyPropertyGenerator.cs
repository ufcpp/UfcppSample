using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Xunit;

namespace Generator.UnitTests;

public class DependencyPropertyGeneratorTests
{
    [Fact]
    public void X()
    {
        RunGenerator("""
            class C
            {
                [DependencyPropertyGenerator.DependencyProperty]
                public partial int P { get; set; }
            }
            """,
            "C.P",
            """
            using System.Windows;

            partial class C
            {
                public static readonly DependencyProperty PProperty =
                    DependencyProperty.Register(
                        nameof(P),
                        typeof(System.Int32),
                        typeof(C),
                        new PropertyMetadata(default(System.Int32)));

                public partial System.Int32 P
                {
                    get => (System.Int32)GetValue(PProperty);
                    set => SetValue(PProperty, value);
                }
            }
            """);
    }

    private static void RunGenerator(
        [StringSyntax("C#")] string targetSource,
        string fileName,
        [StringSyntax("C#")] string generatedSource)
    {
        var parseOptions = new CSharpParseOptions(LanguageVersion.Latest);
        var driver = CSharpGeneratorDriver.Create(new DependencyPropertyGenerator()).WithUpdatedParseOptions(parseOptions);

        var compilation = CSharpCompilation.Create("GeneratorTest",
            references: [MetadataReference.CreateFromFile(typeof(object).Assembly.Location)],
            options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        compilation = compilation.AddSyntaxTrees(CSharpSyntaxTree.ParseText(targetSource, parseOptions));

        driver.RunGeneratorsAndUpdateCompilation(compilation, out var newCompilation, out var diagnostics);

        foreach (var t in newCompilation.SyntaxTrees)
        {
            if (t.FilePath.EndsWith(fileName + ".g.cs"))
            {
                var s = t.GetText().ToString();
                Assert.Equal(generatedSource, s);
            }
        }
    }
}
