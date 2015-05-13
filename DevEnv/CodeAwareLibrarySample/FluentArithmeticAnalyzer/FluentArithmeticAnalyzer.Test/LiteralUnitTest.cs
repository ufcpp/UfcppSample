using FluentArithmeticAnalyzer.Literal;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestHelper;

namespace FluentArithmeticAnalyzer.Test
{
    [TestClass]
    public class LiteralUnitTest : CodeFixVerifier
    {
        [TestMethod]
        public void TestMethod1()
        {
            var test = @"using System;
using FluentArithmetic;

class Program
{
    static void Main(string[] args)
    {
        var x = 1.Add(2).Mul(3).Sub(1).Div(4);
        Console.WriteLine(x);
    }
}
";
            var expected = new DiagnosticResult
            {
                Id = LiteralAnalyzer.DiagnosticId,
                Message = string.Format(LiteralAnalyzer.MessageFormat.ToString(), "Add"),
                Severity = DiagnosticSeverity.Info,
                Locations = new[]
                {
                    new DiagnosticResultLocation("Test0.cs", 8, 17)
                }
            };

            VerifyCSharpDiagnostic(test, expected);

            var newSource = @"using System;
using FluentArithmetic;

class Program
{
    static void Main(string[] args)
    {
        var x = 3.Mul(3).Sub(1).Div(4);
        Console.WriteLine(x);
    }
}
";
            VerifyCSharpFix(test, newSource);
        }

        [TestMethod]
        public void TestMethod2()
        {
            var test = @"using System;
using FluentArithmetic;

class Program
{
    static void Main(string[] args)
    {
        var x = 1.Div(0).Mul(3).Sub(1).Div(4);
        Console.WriteLine(x);
    }
}
";

            VerifyCSharpDiagnostic(test);
        }

        protected override CodeFixProvider GetCSharpCodeFixProvider() => new LiteralFix();

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer() => new LiteralAnalyzer();
    }
}
