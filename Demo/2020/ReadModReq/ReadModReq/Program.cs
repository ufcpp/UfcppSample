using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.IO;

namespace ReadModReq
{
    class Program
    {
        static void Main()
        {
            const string refPath = "../../../../ref/LibModReq.dll";
            var source = File.ReadAllText("../../../../UseModReq.cs");

            var compilation = Compile(refPath, source);

            foreach (var diag in compilation.GetDiagnostics())
            {
                Console.WriteLine(diag);
            }
        }

        private static CSharpCompilation Compile(string refPath, string source)
        {
            var dotnetCoreDirectory = System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory();
            var compilation = CSharpCompilation.Create("test",
                syntaxTrees: new[] { SyntaxFactory.ParseSyntaxTree(source) },
                references: new[]
                {
                    AssemblyMetadata.CreateFromFile(typeof(object).Assembly.Location).GetReference(),
                    MetadataReference.CreateFromFile(Path.Combine(dotnetCoreDirectory, "netstandard.dll")),
                    MetadataReference.CreateFromFile(Path.Combine(dotnetCoreDirectory, "System.Runtime.dll")),
                    MetadataReference.CreateFromFile(refPath),
                });
            return compilation;
        }
    }
}
