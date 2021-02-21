#nullable enable

/// <summary>
/// 制約なし(where T : class とか where T : struct とか付いてない)ジェネリック型引数に対する T? の話。
/// </summary>
/// <typeparam name="T"></typeparam>
class Generic<T>
{
    // C# 9.0 では一応 T? と書ける。
    public T? M() => default;
}
