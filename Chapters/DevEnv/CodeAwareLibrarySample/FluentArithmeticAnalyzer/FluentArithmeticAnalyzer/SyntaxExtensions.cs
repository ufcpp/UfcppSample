using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace FluentArithmeticAnalyzer
{
    internal static class SyntaxExtensions
    {
        public static ISymbol GetFluentExtensionsMember(this InvocationExpressionSyntax expr, SyntaxNodeAnalysisContext context)
        {
            var ma = expr.Expression as MemberAccessExpressionSyntax;
            if (ma == null) return null;

            var method = context.SemanticModel.GetMemberGroup(ma).FirstOrDefault();
            if (method == null) return null;

            var ct = method.ContainingType;
            if (ct.ContainingNamespace.Name != "FluentArithmetic" || ct.Name != "FluentExtensions") return null;

            return method;
        }

        public static LiteralExpressionSyntax GetLiteralArgument(this InvocationExpressionSyntax expr)
        {
            if (expr.ArgumentList == null) return null;

            var args = expr.ArgumentList.Arguments;
            if (args.Count != 1) return null;

            var arg = args[0].Expression;
            if (!arg.IsKind(SyntaxKind.NumericLiteralExpression)) return null;

            return arg as LiteralExpressionSyntax;
        }

        public static LiteralExpressionSyntax GetLiteralExpression(this InvocationExpressionSyntax expr)
        {
            var ma = expr.Expression as MemberAccessExpressionSyntax;
            if (ma == null) return null;

            var obj = ma.Expression;
            if (!obj.IsKind(SyntaxKind.NumericLiteralExpression)) return null;

            return obj as LiteralExpressionSyntax;
        }

        public static string GetMemberName(this InvocationExpressionSyntax expr)
        {
            var ma = expr.Expression as MemberAccessExpressionSyntax;
            if (ma == null) return null;

            return ma.Name.Identifier.ValueText;
        }
    }
}
