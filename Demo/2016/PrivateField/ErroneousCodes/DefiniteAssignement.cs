struct EmptyStruct { }
struct Integer { private int _x; }

struct DefiniteAssignement
{
    EmptyStruct _e;
    Integer _i;

    DefiniteAssignement(int i)
    {
        // 中身があるものは初期化必須
        _i = new Integer();
        // 一方で、EmptyStruct みたいに空っぽのものは初期化不要
    }
}
