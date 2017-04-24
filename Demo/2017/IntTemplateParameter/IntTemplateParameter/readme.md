# C++ の `template<int N>` もどきを C# で

※過度の期待はしないでください

## 整数値テンプレート引数

C++ の template でしかできないことの1つに、テンプレート引数に int を取れることがある。
有名なのが、数論系のライブラリで、ガロアの有限体を作る例。C++ だと、

```cpp
template<int N>
class GaloisField
{
}
```

みたいな定義で、`GaloisField<2>` とか `GaloisField<3>` とか(ぞれぞれ別の型扱いされる)を作れる。

## C# の値型ジェネリクス最適化

詳しくはこの辺りを: https://github.com/ufcpp/UfcppSample/tree/master/Demo/2017/InlineExpansion

C# のジェネリクスでは、整数値を直接ジェネリック引数に取れない(「型引数」って呼ばれるように、型しかパラメーター化できない)。
けど、C# のジェネリクスの以下のような性質を使って、疑似的な整数値ジェネリック引数ができるんじゃないかという発想。

- 型引数に値型を渡すと、型毎に個別展開が行われる
- 値型に対しては、`default(T).Method()` とかで、インスタンスを作らなくてもメソッドなどのメンバーを呼べる
- virtual ではない小さい関数メンバーは、インライン展開が掛かって高速かできる

## このプロジェクトでやっていること

要するに、定数引数の代わりに、「定数っぽい挙動をする型引数」を使ってごまかす。

以下のC++コードの代わりに、

```cpp
template<int N>
class GaloisField
{
    // N で有限体のモジュロを表現
}
```

以下のようなC#コードを用意する。

```cs
struct GaloisField<N>
    where N : IConstant<N>
{
    // default(N).Value で有限体のモジュロを表現
}
```

`N`に対して直接整数値を渡せないので、代わりに、以下のような型を用意。

```cs
public static class ConstantInt
{
    public struct _0 : IConstant<int> { public int Value => 0; }
    public struct _1 : IConstant<int> { public int Value => 1; }
    public struct _2 : IConstant<int> { public int Value => 2; }
    ...
}
```

C++ の `GaloisField<2>` とかの代わりに、`GaloisField<ConstantInt._2>` で、モジュロ2の有限体が作れる。

## 制限事項

仕組み上、`struct _0` みたいな型を大量に作らないといけない。
今回の例では、T4 テンプレートを使って、0～64までを生成。それ以上の値は未提供。

あと、定数(C#言語文法のconst)と比べるとやっぱりちょっと最適化掛かりにくいはず。
