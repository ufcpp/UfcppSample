// record (record class) と record struct の使い分け、概ね通常の class と struct の関係と一緒。

// 1. ヒープ アロケーション

int N = 1000;

for (int i = 0; i < N; i++)
{
    // クラスの場合、
    // こういうループの内側で使い捨てインスタンスを new しまくるのはあんまりよろしくない。
    // GC 誘発。
    var c = new RecordClass(1, 2);
}

for (int i = 0; i < N; i++)
{
    // 同じようなことをやってても、構造体ならそこまでの負担はない。
    var x = new RecordStruct(1, 2);

    // なんだったら、.NET 6 ではこの手のコードのインライン展開が掛かって、
    // 完全に new RecordStruct() と Abs 呼び出しが消滅したりする。
    // (C# コンパイル時最適化じゃなくて、.NET ランタイム JIT 時最適化で。)
    var absx = Math.Abs(x.X);
}

// 2. デフォルト値(配列の要素とか)

// クラスの場合、null 初期化。
var a1 = new RecordClass[N];

// この状態はぬるぽ。
//Console.WriteLine(a1[0].X);

// なので null チェックが必要。
// (プロパティ パターン {} は null チェックを含んでる。)
if (a1[0] is { X: var a0x }) Console.WriteLine(a0x);

// 構造体の場合、0 初期化。
var a2 = new RecordStruct[N];

// なのでこれは 0 が表示される。
Console.WriteLine(a2[0].X);

// 3. 参照型か値型か以外の差

// C# 7.0/7.2 で ref 戻り値、in 引数が増えて以来、「構造体は mutable にしてもよい」という風潮。
// なので、(readonly を付けてない) record struct は get; set; でプロパティ生成されてる。
// 匿名型 new { X = 1, ... } とタプル (X: 1, ...) の差に近い。

var rc = new RecordStruct(1, 2);
rc.X = 3; // これは OK。

var rs = new RecordClass(1, 2);
//rs.X = 3; // これはコンパイル エラー。

// タプルは構造体 = record struct に扱いが近い。
var tuple = (X: 1, Y: 2);
tuple.X = 3; // これは OK。

// 匿名型は参照型(クラス)として生成されてる = record class に扱いが近い。
var anonymous = new { X = 1, Y = 2 };
//anonymous.X = 3; // これはコンパイル エラー。

// 型定義
record struct RecordStruct(int X, int Y);
record class RecordClass(int X, int Y);
