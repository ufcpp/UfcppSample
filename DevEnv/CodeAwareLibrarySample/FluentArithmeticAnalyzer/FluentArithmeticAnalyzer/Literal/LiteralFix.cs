using System;
using System.Composition;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Rename;
using Microsoft.CodeAnalysis.Text;

namespace FluentArithmeticAnalyzer.Literal
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(LiteralFix)), Shared]
    public class LiteralFix : CodeFixProvider
    {
        public sealed override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(LiteralAnalyzer.DiagnosticId);

        public sealed override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken);
            var expr = root.FindNode(context.Span).FirstAncestorOrSelf<InvocationExpressionSyntax>();

            if (expr != null)
            {
                context.RegisterCodeFix(
                    CodeAction.Create("Optimize to one literal",
                        c => ChangeToOneLiteral(expr, context.Document, c)),
                    context.Diagnostics[0]);
            }
        }

        private async Task<Document> ChangeToOneLiteral(InvocationExpressionSyntax ex, Document document, CancellationToken cancellationToken)
        {
            var literal1 = ex.GetLiteralExpression();
            if (literal1 == null) return document;

            var literal2 = ex.GetLiteralArgument();
            if (literal2 == null) return document;

            var x = int.Parse(literal1.Token.ValueText);
            var y = int.Parse(literal2.Token.ValueText);
            var newValue = 0;

            switch (ex.GetMemberName())
            {
                case "Add": newValue = x + y; break;
                case "Sub": newValue = x - y; break;
                case "Mul": newValue = x * y; break;
                case "Div": newValue = x / y; break;
            }

            var newLiteral = SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, SyntaxFactory.Literal(newValue))
                .WithLeadingTrivia(ex.GetLeadingTrivia())
                .WithTrailingTrivia(ex.GetTrailingTrivia())
                ;

            var oldRoot = await document.GetSyntaxRootAsync(cancellationToken);
            var newRoot = oldRoot.ReplaceNode(ex, newLiteral);

            return document.WithSyntaxRoot(newRoot);
        }
    }
}