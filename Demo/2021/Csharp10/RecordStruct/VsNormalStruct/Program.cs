// C# 9.0 で追加されたレコード型は思想的には「構造体的な扱いができる参照型」だったり。
// C# 10.0 で record struct (構造体もレコードにできる)と言うのが入って、「構造体的な扱いができる構造体？」みたいになってる。
// 「通常の struct に record と同程度の機能性を持たせるかどうか」みたいな議論もあったものの、
// 結局、「元々の struct と、record (record class) の差を埋めるために record struct を新設」という流れに。
//
// ということで、record struct と(元からある通常の) struct の差を紹介。

// 1. プライマリ コンストラクターを持てるのは record struct だけ。
// 正確に言うと、(将来計画として)通常のクラス/構造体にもプライマリ コンストラクターを認めるかもしれないものの、
// その場合でも「プライマリ コンストラクターの引数からプロパティをコンパイラー生成」するのは record を付けた時だけ。

var r = new RecordStruct(1, 2);
Console.WriteLine(r.X); // コンストラクター引数からプロパティが生成されてる。

// 2. ↑の一環として、Deconstruct も生成されてる。
var (x, _) = r;
Console.WriteLine(x);

// 3. IEquatable<T> が実装されてる。
// 通常の構造体は Equals(object? obj) は持ってるけど、Equals(T other) は持ってない。
// これも正確に言うと通常の構造体の Equals(object? obj) は object.Equals をそのまま使ってる(override はしてない)。
// object.Equals の中で構造体の特殊扱いが働いていて、「全メンバーのビット列が一致してるときに true」になってる。
//
// 一方で、record struct のコンパイラー生成の Equals はメンバーごとに EqualityComparer<T>.Default.Equals を呼んでる。
// object.Equals よりも速いらしい。
Console.WriteLine(r.Equals(new RecordStruct(2, 3))); // RecordStruct.Equals(RecordStruct) が呼ばれてる。

Console.WriteLine(r == new RecordStruct(1, 2)); // ちなみに、== と != も生成されてる。

// 4. ↑の余波で、IEquatable<T> 実装、EqualityComparer<T> 使用と両立しない機能は使えない。
//ref record struct RefStruct; // ref struct がインターフェイス実装と両立しないのでダメ。
//record struct Pointer(int* X); // ポインターが EqualityComparer<T> と両立しないのでダメ。

// 型定義
record struct RecordStruct(int X, int Y);
