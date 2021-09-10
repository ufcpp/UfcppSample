using System.Runtime.CompilerServices;

// 普通(?)
// (C# にしては珍しいけど、「InterpolatedStringHandler は string より優先する」という仕様にした。)
// (IFormattable の時の反省。)

new Instance().M(""); // string
new Instance().M($"{1}"); // handler

new StringIsExtension().M(""); // string
new StringIsExtension().M($"{1}"); // handler

new BothExtension().M(""); // string
new BothExtension().M($"{1}"); // handler

new DerivedFromBaseString().M(""); // string
new DerivedFromBaseString().M($"{1}"); // handler

// 特殊。
// (InterpolatedStringHandler を優先できない状況。)

// インスタンスメソッドを優先。
new HandlerIsExtension().M(""); // string
new HandlerIsExtension().M($"{1}"); // string これは特殊

// 派生クラスのメソッドを優先。
new DerivedFromBaseHandler().M(""); // string
new DerivedFromBaseHandler().M($"{1}"); // string これは特殊

class Instance
{
    public void M(string _) => Console.WriteLine("string");
    public void M(DefaultInterpolatedStringHandler _) => Console.WriteLine("handler");
}

class HandlerIsExtension
{
    // 特殊。拡張メソッドの M(DefaultInterpolatedStringHandler _) より優先。
    public void M(string _) => Console.WriteLine("string");
}

class StringIsExtension
{
    public void M(DefaultInterpolatedStringHandler _) => Console.WriteLine("handler");
}

class BothExtension { }

class BaseString
{
    public void M(string _) => Console.WriteLine("string");
}
class DerivedFromBaseString : BaseString
{
    public void M(DefaultInterpolatedStringHandler _) => Console.WriteLine("handler");
}

class BaseHandler
{
    public void M(DefaultInterpolatedStringHandler _) => Console.WriteLine("handler");
}
class DerivedFromBaseHandler : BaseHandler
{
    // 特殊。基底クラスの M(DefaultInterpolatedStringHandler _) より優先。
    public void M(string _) => Console.WriteLine("string");
}

static class Extensions
{
    public static void M(this HandlerIsExtension _, DefaultInterpolatedStringHandler _1) => Console.WriteLine("handler");
    public static void M(this StringIsExtension _, string _1) => Console.WriteLine("string");
    public static void M(this BothExtension _, DefaultInterpolatedStringHandler _1) => Console.WriteLine("handler");
    public static void M(this BothExtension _, string _1) => Console.WriteLine("string");
}
