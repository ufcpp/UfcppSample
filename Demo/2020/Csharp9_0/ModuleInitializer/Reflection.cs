#if false

// 方式1: リフレクション

using System;
using System.Reflection;

// リフレクションを使えば文字列からその名前の型のインスタンスを作れる。
// ただ、パフォーマンスはあんまりよくない。
object? CreateInstance(string typeName)
{
    if (Assembly.GetExecutingAssembly().GetType(typeName) is { } t) return Activator.CreateInstance(t);
    else return null;
}

// ただ、 "A", "B" という文字列が型名を指しているかどうかはコンパイラーが関知することではなく、
// 「クラス A, B は誰も使っていない」誤判定を受けることがある。
// AOT (事前ネイティブコード化)実行環境だと A, B が消し去られて、上記 GetType に失敗しうる。
Console.WriteLine(CreateInstance("A"));
Console.WriteLine(CreateInstance("B"));

class A
{
}

class B
{
}

#endif
