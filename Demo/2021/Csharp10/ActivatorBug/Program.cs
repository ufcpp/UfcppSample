// このコード、 .NET Core で実行すると全部1が表示されるけど、
// .NET Framework で実行すると1つだけ1、2つ目以降は0が表示される。
// net48 (一応最新の .NET Framework) ですらバグ挙動。

using System;

Console.WriteLine(Activator.CreateInstance<T>().Value);
Console.WriteLine(Activator.CreateInstance<T>().Value);
Console.WriteLine(New<T>().Value);
Console.WriteLine(New<T>().Value);

static T New<T>() where T : new() => new();

struct T
{
    public int Value;
    public T() => Value = 1;
}
