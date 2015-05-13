using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace FluentArithmeticAnalyzer.Literal
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class LiteralAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "LiteralAnalyzer";
        internal static readonly LocalizableString Title = "Arithmetic operation on two literals";
        internal static readonly LocalizableString MessageFormat = "Call the {0} with two literals";
        internal const string Category = "Correction";

        internal static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Info, true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

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
                if (method.Name == "Div" && literal.Token.ValueText == "0") continue;

                var obj = ex.GetLiteralExpression();
                if (obj == null) continue;

                var diagnostic = Diagnostic.Create(Rule, ex.GetLocation(), method.Name);
                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}