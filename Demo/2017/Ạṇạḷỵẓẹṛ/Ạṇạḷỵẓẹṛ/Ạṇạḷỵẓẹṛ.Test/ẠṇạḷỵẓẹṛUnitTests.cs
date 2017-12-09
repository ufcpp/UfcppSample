using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using System;
using TestHelper;
using Xunit;
using Ạṇạḷỵẓẹṛ;

namespace Ạṇạḷỵẓẹṛ.Test
{
    public class ẠṇạḷỵẓẹṛUnitTests : ConventionCodeFixVerifier
    {
        [Fact]
        public void EmptySource() => VerifyCSharpByConvention();

        [Fact]
        public void LowercaseLetters() => VerifyCSharpByConvention();

        protected override CodeFixProvider GetCSharpCodeFixProvider() => new ẠṇạḷỵẓẹṛCodeFixProvider();

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer() => new ẠṇạḷỵẓẹṛAnalyzer();
    }
}
