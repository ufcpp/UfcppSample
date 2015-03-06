enum X : System.Int32
{
    A, B, C,
}

/// <summary>
/// Can be compiled in: C# 6
///
/// Changed: enum base type can be a type expression like System.Int32
/// </summary>
/// <remarks>
/// The ealier versions produce:
/// error CS1008: Type byte, sbyte, short, ushort, int, uint, long, or ulong expected
/// </remarks>
class EnumBaseTypeSystemInt32 { static void Main() { } }
