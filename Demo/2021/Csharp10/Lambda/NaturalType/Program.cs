// C# 9.0 までは
// Func<string, int> f = m;
// みたいに書かないとダメだった(ターゲット型推論)。
var f = m;
Delegate d = m;
MulticastDelegate md = m;

// Delegate は ICloneable を実装しているので一応これも OK。
// (ただし、ICloneable インターフェイス自体今どき使わない。)
ICloneable c = m;

// これも一応できるけど、そんなに使い道がないというかたまにミスの原因になるので警告。
// object obj = m(); の () 付け忘れをたまにやるので…
object obj = m;

int m(string s) => s.Length;

// System.Action とかになるやつ:
var a1 = (int a) => { };
var a4 = (int a, int b, int c, int d) => { };
var a16 = (int a, int b, int c, int d, int e, int f, int g, int h, int i, int j, int k, int l, int m, int n, int o, int p) => { };
var f1 = (int a) => a.ToString();
var f4 = (int a, int b, int c, int d) => $"{a}.{b}.{c}.{d}";

// コンパイラー生成の独自デリゲートになるやつ:
// ref 系
var i1 = (in int a) => { };
var r1 = (ref int a) => { };
var o1 = (out int a) => a = 0;
// 引数の数オーバー
var a17 = (int a, int b, int c, int d, int e, int f, int g, int h, int i, int j, int k, int l, int m, int n, int o, int p, int q) => { };

#if ERROR
// Func<int, bool> になる。
var a = (int x) => true;

// 左辺に型を明示してあると Action/Func 以外の型になる。
Predicate<int> p = (int x) => true;

// p に直接 (int x) => true を代入するのは行けるのに、
// var 変数宣言を挟むとダメ。
// (Func<int, bool> から Predicate<int> への変換が許されていない。)
p = a;
#endif
