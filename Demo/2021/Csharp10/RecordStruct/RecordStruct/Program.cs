// Point が X, Y プロパティを持ってる(get; set;)
using System.Reflection;

var p1 = new Point(1, 2);
p1.X = 3; // 普通に書き換え可能。
Console.WriteLine(p1); // ToString とかは record class と一緒。

// 一応リフレクションで確認。
var t = typeof(Point);

foreach (var p in t.GetProperties())
{
    var isInitOnly = p.GetSetMethod()?.ReturnParameter.GetRequiredCustomModifiers().Any(x => x.Name == "IsExternalInit");
    Console.WriteLine((p.Name, isInitOnly));
}

// readonly が付いていない record struct でも、プロパティの get メソッドとかには readonly が付く。

static bool isReadOnly(MethodInfo? method)
    => method?.CustomAttributes.Any(a => a.AttributeType.Name == "IsReadOnlyAttribute") == true;

Console.WriteLine(isReadOnly(t.GetProperty("X")?.GetGetMethod()));

// PrintMembers、GetHashCode とかにも付くはず(.NET 6 Preview 7 時点ではまだっぽい？)
//Console.WriteLine(isReadOnly(t.GetMethod("PrintMembers", BindingFlags.NonPublic | BindingFlags.Instance)));
//Console.WriteLine(isReadOnly(t.GetMethod("GetHashCode")));

// ReadOnlyPoint も X, Y プロパティを持ってるけど、get; init;
var p2 = new ReadOnlyPoint(1, 2)
{
    X = 3, // init-only なのでこれは OK。
};
//p2.X = 3; // これはダメ。
Console.WriteLine(p2);

// 一応リフレクションで確認。
foreach (var p in typeof(ReadOnlyPoint).GetProperties())
{
    var isInitOnly = p.GetSetMethod()?.ReturnParameter.GetRequiredCustomModifiers().Any(x => x.Name == "IsExternalInit");
    Console.WriteLine((p.Name, isInitOnly));
}

// ほぼ (classs の) record と一緒。
// X, Y に対応するプロパティが生成される。
// ただ、普通の record struct で書くと get; set; なプロパティが生成される。
// (readonly/init-only にならない。タプルと一緒。構造体は immutable である必要性がクラスよりは低い。)
record struct Point(int X, int Y);

// readonly record struct にすると get; init; なプロパティが生成される。
readonly record struct ReadOnlyPoint(int X, int Y);

// record class って書けるようになった。
// record struct との対称性のために足しただけで、C# 9.0 の "record" と全く一緒。
record class ClassPoint(int X, int Y);
