#pragma warning disable CS8321

Console.WriteLine("Hello, World!");

static A m1()
{
    var a = new A();
    return a;
}

static A m2(ref int x)
{
    var a = new A() { X = ref x };
    return a;
}

#if ERROR
static A m3()
{
    var x = 1;
    var a = new A();
    a.X = ref x; // エラー: local x を scoped じゃない a に代入。
    return a; // その代わり、return しても OK。
}
#endif

static void m4()
{
    var x = 1;
    scoped var a = new A();
    a.X = ref x; // これは OK。
}

#if ERROR
static A m5()
{
    var x = 1;
    scoped var a = new A { X = ref x };
    return a; // エラー: scoped a を return。
}
#endif

ref struct A { public ref int X; }
