using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Generator;

[Generator]
public class DependencyPropertyGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(ctx => ctx.AddSource(
            "DependencyPropertyAttribute.g.cs",
            /* lang=C# */
            """
            using System;

            namespace DependencyPropertyGenerator;

            [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
            public class DependencyPropertyAttribute : Attribute
            {
            }
            """));

        var propertyDeclarations = context.SyntaxProvider.ForAttributeWithMetadataName(
            "DependencyPropertyGenerator.DependencyPropertyAttribute",
            (node, _) => IsPartialProperty(node),
            (context, _) => GetSemanticTargetForGeneration(context));

        context.RegisterSourceOutput(propertyDeclarations, static (spc, source) => Execute(source, spc));
    }

    private static bool IsPartialProperty(SyntaxNode node)
    {
        return node is PropertyDeclarationSyntax propertyDeclaration &&
               propertyDeclaration.Modifiers.Any(SyntaxKind.PartialKeyword);
    }

    private static PropertyInfo GetSemanticTargetForGeneration(GeneratorAttributeSyntaxContext context)
    {
        var p = (PropertyDeclarationSyntax)context.TargetNode;
        var t = context.SemanticModel.GetTypeInfo(p.Type).Type!;
        var ct = context.SemanticModel.GetDeclaredSymbol(p)!.ContainingType;

        var pi = new PropertyInfo(
            p.Identifier.Text,
            t.ContainingNamespace + "." + t.Name,
            ct.Name,
            ct.ContainingNamespace.Name);

        return pi;
    }

    private static void Execute(PropertyInfo? propertyInfo, SourceProductionContext context)
    {
        if (propertyInfo is null)
            return;

        var source = GenerateSource(propertyInfo);
        context.AddSource($"{propertyInfo.ClassName}.{propertyInfo.PropertyName}.g.cs", source);
    }

    private static string GenerateSource(PropertyInfo propertyInfo)
    {
        var (propertyName, propertyType, className, namespaceName) = propertyInfo;

        var namespaceDeclaration = string.IsNullOrEmpty(namespaceName)
            ? string.Empty
            : $"""
            namespace {namespaceName};


            """;

        return $$"""
            using System.Windows;

            {{namespaceDeclaration}}partial class {{className}}
            {
                public static readonly DependencyProperty {{propertyName}}Property =
                    DependencyProperty.Register(
                        nameof({{propertyName}}),
                        typeof({{propertyType}}),
                        typeof({{className}}),
                        new PropertyMetadata(default({{propertyType}})));

                public partial {{propertyType}} {{propertyName}}
                {
                    get => ({{propertyType}})GetValue({{propertyName}}Property);
                    set => SetValue({{propertyName}}Property, value);
                }
            }
            """;
    }

    private record PropertyInfo(
        string PropertyName,
        string PropertyType,
        string ClassName,
        string Namespace);
}
