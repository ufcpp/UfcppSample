// 引数の int から、戻り値の型が int に決定する。
// その後、ラムダ式の型は Func<int, int> として決定できる。
var f = (int x) => x;

// 条件演算子だけでは int と null の共通型が決定できなくて、戻り値の型が決まらない。
// (条件演算子の後方互換性のために掛かってる制限。)
#if ERROR
var f1 = (bool x, int y) => x ? y : null;
#endif

// 一方で、これなら、戻り値の型からのターゲット型推論で条件演算子を書ける。
// f2 の自然な型決定もできるようになる (Func<bool, int, int?> になる)。
var f2 = int? (bool x, int y) => x ? y : null;
