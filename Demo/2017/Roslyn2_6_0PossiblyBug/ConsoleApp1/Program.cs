using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

public struct ExternalStruct { }

class Program
{
    static void Main()
    {
        var cu = ParseCompilationUnit(@"
public class X
{
    public ExternalStruct e;
    public InternalStruct i;
    public System.DateTime d;
}

public struct InternalStruct { }
");

        var sem = GetSemantics(cu);

        var classX = (ClassDeclarationSyntax)cu.Members[0];

        // X.e (struct ExternalStruct)
        ShowTypeInfo(sem, (FieldDeclarationSyntax)classX.Members[0]);

        // X.i (struct InternalStruct)
        ShowTypeInfo(sem, (FieldDeclarationSyntax)classX.Members[0]);

        // X.d (struct DateTime)
        ShowTypeInfo(sem, (FieldDeclarationSyntax)classX.Members[2]);

/* results
# with net47:
Struct ExternalStruct
Struct ExternalStruct
Struct DateTime

# with netcoreapp2.0:
Class ExternalStruct
Class ExternalStruct
Struct DateTime
*/
    }

    private static void ShowTypeInfo(SemanticModel sem, FieldDeclarationSyntax field)
    {
        var t = sem.GetTypeInfo(field.Declaration.Type).Type;

        Console.WriteLine($"{t.TypeKind} {t.Name}");
    }

    private static SemanticModel GetSemantics(CompilationUnitSyntax cu)
    {
        var compilation = CSharpCompilation.Create("sample")
            .AddReferences(
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Program).Assembly.Location))
            .AddSyntaxTrees(cu.SyntaxTree);

        var sem = compilation.GetSemanticModel(cu.SyntaxTree);
        return sem;
    }
}
