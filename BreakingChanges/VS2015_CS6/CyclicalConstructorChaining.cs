class C
{
    public C(int x) : this() { }
    public C() : this(0) { }
}

// In C# it is a compile-time error to write a constructor that recursively chains directly to itself
//class C1
//{
//    public C(int x) : this(x + 1) { }
//}

class CyclicalConstructorChaining
{
    static void Main() { }
}
