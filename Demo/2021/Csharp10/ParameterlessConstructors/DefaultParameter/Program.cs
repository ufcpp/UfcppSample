Console.WriteLine(new Ctor());

#if Error
void m(
    NoCtor n1 = new(),
    NoCtor n2 = default,
    Ctor c1 = new(), // この行だけコンパイル エラー
    Ctor c2 = default
    )
{ }
#endif

struct NoCtor { }
struct Ctor { public Ctor() { } }
