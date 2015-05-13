using FluentArithmeticAnalyzer.DivByZero;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestHelper;

namespace FluentArithmeticAnalyzer.Test
{
    [TestClass]
    public class DivByZeroUnitTest : CodeFixVerifier
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

            VerifyCSharpDiagnostic(test);
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
        var x = 1.Add(2).Mul(3).Sub(1).Div(0);
        Console.WriteLine(x);
    }
}
";
            var expected = new DiagnosticResult
            {
                Id = DivByZeroAnalyzer.DiagnosticId,
                Message = DivByZeroAnalyzer.MessageFormat.ToString(),
                Severity = DiagnosticSeverity.Error,
                Locations = new[]
                {
                    new DiagnosticResultLocation("Test0.cs", 8, 44)
                }
            };

            VerifyCSharpDiagnostic(test, expected);
        }

        protected override CodeFixProvider GetCSharpCodeFixProvider() => null;

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer() => new DivByZeroAnalyzer();
    }
}