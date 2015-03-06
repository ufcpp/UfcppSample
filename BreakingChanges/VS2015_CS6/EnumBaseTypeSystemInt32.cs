enum X : System.Int32
{
    A, B, C,
}

/// <summary>
/// Can be compiled in: C# 6
///
/// enum base type can be a type expression like System.Int32
/// </summary>
class EnumBaseTypeSystemInt32 { static void Main() { } }
