using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace FluentArithmeticAnalyzer.DivByZero
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class DivByZeroAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "DivByZeroAnalyzer";
        internal static readonly LocalizableString Title = "Call 'Div' by 0";
        internal static readonly LocalizableString MessageFormat = "Do not call the 'Div' method with 'y' being 0.";
        internal const string Category = "Correction";

        internal static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Error, true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(AnalyzeMethod, SyntaxKind.MethodDeclaration);
        }

        private void AnalyzeMethod(SyntaxNodeAnalysisContext context)
        {
            var invocations = context.Node
                .DescendantNodes()
                .OfType<InvocationExpressionSyntax>();

            foreach (var ex in invocations)
            {
                var method = ex.GetFluentExtensionsMember(context);
                if (method == null) continue;

                var literal = ex.GetLiteralArgument();
                if (literal == null) continue;

                if (method.Name == "Div" && literal.Token.ValueText == "0")
                {
                    var diagnostic = Diagnostic.Create(Rule, literal.GetLocation());
                    context.ReportDiagnostic(diagnostic);
                }
            }
        }
    }
}
