#:package Microsoft.CodeAnalysis.CSharp@4.14.0

using Microsoft.CodeAnalysis.CSharp;

var tree = CSharpSyntaxTree.ParseText("class Class1;");
var root = await tree.GetRootAsync();
Console.WriteLine(root.GetFirstToken().Text);
