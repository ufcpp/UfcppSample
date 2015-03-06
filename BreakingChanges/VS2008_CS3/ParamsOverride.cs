interface I
{
    int this[int[] p] { set; }
}

class Base : I
{
    // Produces CS0466:
    int I.this[params int[] p] { set { } }
}

/// <summary>
/// Changed in VS 2008 SP1
/// Can be compiled in: C# 2, 3 (no SP1)
///
/// #2 in https://msdn.microsoft.com/en-us/library/cc713578.aspx
/// Compiler now produces error CS0466 for indexers and properties in addition to methods.
/// </summary>
/// <remarks>
/// For methods, All versions of C# compilers produce CS0466, which meets the specification. This breaking change for indexers is a bug fix.
/// <code><![CDATA[
/// interface I
/// {
///     void F(int[] p);
/// }
/// 
/// class Base : I
/// {
///     // Produces CS0466:
///     void I.F(params int[] p) { }
/// }
/// ]]></code>
///
/// FYI, it is legal if the indexer is implicitly implemented:
/// <code><![CDATA[
/// interface I
/// {
///     int this[int[] p] { set; }
/// }
/// 
/// class Base : I
/// {
///     public int this[params int[] p] { set { } }
/// }
/// ]]></code>
/// </remarks>
class ParamsOverride { static void Main() { } }
