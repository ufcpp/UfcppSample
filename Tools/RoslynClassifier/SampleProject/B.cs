using System.Diagnostics.CodeAnalysis;
using A = SampleProject.A;

namespace SampleProject;

internal class B
{
    public B(A a) { }

    [return: NotNullIfNotNull("s")]
    public static int? M(string? s) => s?.Length;
}
