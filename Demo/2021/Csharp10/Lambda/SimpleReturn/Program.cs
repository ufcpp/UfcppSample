// 新文法。
// ラムダ式に戻り値の型を明示。
// (引数も明示。)
Func<int, int> f1 = int (int x) => x;

// 元々の文法。
// 引数の型の方を明示。
var f2 = (int x) => x;

// 新文法。
// 戻り値の型だけ明示。 () が必要。
Func<int, int> f3 = int (x) => x;

// これはエラーになる。
// int が引数に掛かっているのか戻り値に掛かっているのか不明瞭。
#if ERROR
Func<int, int> f4 = int x => x;
#endif

// 戻り値の型を各場所は static の後ろ。
var f5 = static int (int x) => x;

// 戻り値の型から引数の型の推論はできない。
// 結果的に、Func<T, int> への代入はできても、自然な型決定(var などへの代入)はできない。
#if ERROR
var f6 = int (x) => x;
#endif
