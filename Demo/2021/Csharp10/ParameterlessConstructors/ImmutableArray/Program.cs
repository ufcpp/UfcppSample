// これまでだと _array?.Length ?? 0 みたいな分岐が必要だったものの、
// 今なら「引数なしコンストラクターで非 null に初期化されてるはず」みたいな前提のコードが書けはする。
var a1 = new ImmutableArray<int>();
Console.WriteLine(a1.Length);

// ただし…
// default を使うといまだ「コンストラクターを呼ばずに 0 初期化」になるので、
a1 = default;
Console.WriteLine(a1.Length); // ここでぬるぽ。

// 構造体に対する default は参照型に対する null と同程度に厄介者に。

struct ImmutableArray<T>
{
    private readonly T[] _array;

    // 引数なしコンストラクターから : this(...) で別のコンストラクターを呼び出すことも可能。
    // これで必ず _array = new T[length] されるかと言うと…
    public ImmutableArray() : this(0) { }
    public ImmutableArray(int length) => _array = new T[length];

    public T this[int index] => _array[index];
    public int Length => _array.Length;
}
