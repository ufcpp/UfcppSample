using System;
using System.Collections.Generic;

// どこか必ず1回呼ばれる保証のあるものを使って、事前に string → Func<object> な辞書を作っておくという発想。
static class TypeRepository
{
    private static readonly Dictionary<string, Func<object>> _factories = new();

    // 型名からインスタンスを作る。Register がどこかで呼ばれる前提。
    public static object? CreateInstance(string typeName) => _factories.TryGetValue(typeName, out var f) ? f() : null;

    // 型名 → インスタンス生成デリゲートを登録。
    // 静的コンストラクターで呼んでもらう想定だと破綻気味だったけど、モジュール初期化子なら割と成立する。
    public static void Register(string typeName, Func<object> factory) => _factories.Add(typeName, factory);
}
