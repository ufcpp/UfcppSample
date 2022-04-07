// C# 10 の頃から実装はあったけども、最終的に LangVersion 10 には入らず、C# 10 正式リリース時点で Preview として残ったもの。
// (.NET ランタイム側の対応が必要で、テストが間に合わなかった(実際何か不具合あったっぽい？)だけ。)
// (現在の計画では) C# 11 で今度こそ正式に入る予定。

// 属性をジェネリック型にできるようになりました。以上。
[AttributeUsage(AttributeTargets.All)]
sealed class GenericAttribute<T> : Attribute { }

// 通常の属性と同様、Attribute 語尾は省略できる。
[Generic<int>]
class A { }

// フルネームでも OK。
[GenericAttribute<int>]
class B { }

// 型引数が open なままではダメ。
// 例えば以下のコードはコンパイルエラーになる。
#if false
[GenericAttribute<T>] // CS8968 エラー
class C<T> { }
#endif
