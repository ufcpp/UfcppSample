namespace ParameterlessConstructors.RecordStruct;

// record (class) で以下のような書き方できるんだし、record struct でも書けるようにしたい。
// そのためには引数なしコンストラクターが必要だった。

record struct A();

readonly record struct B
{
    private static int _id;
    private static int GetId() => Interlocked.Increment(ref _id);

    public int Id { get; } = GetId();
}
