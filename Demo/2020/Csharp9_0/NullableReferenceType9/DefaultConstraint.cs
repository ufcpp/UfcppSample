#nullable disable

// さかのぼること、null 許容参照型導入前にから以下のような書き方ができた。
class Csharp7
{
    // これは Nullable<T> の意味に。
    public T? M<T>(T? x) where T : struct => null;

    // T と Nullable<T> は別の型扱いなのでオーバーロード可能。
    public T M<T>(T x) => default;
}

#nullable enable

// ここで、null 許容参照型を有効化。
// 特に、C# 9.0 では制約なし型引数に対して T? と書けるようになったので…
class Base
{
    // これは Nullable<T> の意味に。
    public virtual T? M<T>(T? t) where T : struct => null;

    // これは C# 9.0 の制約なし型引数に対する null 許容(正確には default 許容)アノテーション。
    // T と Nullable<T> 違いのオーバーロードという扱いになる。
    public virtual T? M<T>(T? t) => default;
}

// さらに紛らわしいのが↑を override したときで…
class Derived : Base
{
    // これ、実は Nullable<T> の意味。
    // 親クラス側の where T : struct 制約を自動的に引き継いでしまう。
    // こうしないと C# 8.0 以前との整合性が取れないとのこと。
    public override T? M<T>(T? t) => null;

    // ということで、制約なし T? の方を参照するために別の制約が必要になったという経緯があり。
    // override 時に限り、where T : struct じゃない方に、逆に where T : default という制約を書く必要がある。
    public override T? M<T>(T? t) where T : default => default;
}
