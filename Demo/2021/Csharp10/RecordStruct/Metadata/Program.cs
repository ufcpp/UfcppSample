using System.Reflection;

// 定義してる型は RecordStruct.csproj と一緒。
// リフレクションで中身の確認。

// record class には必ず EqualityContract と <Clone>$ がある。
var typeofClass = typeof(ClassPoint);

Console.WriteLine(typeofClass.GetMethod("<Clone>$"));
Console.WriteLine(typeofClass.GetProperty("EqualityContract", BindingFlags.NonPublic | BindingFlags.Instance));

// Point が X, Y プロパティを持ってる(get; set;)
var typeofStruct = typeof(Point);

foreach (var p in typeofStruct.GetProperties())
{
    var isInitOnly = p.GetSetMethod()?.ReturnParameter.GetRequiredCustomModifiers().Any(x => x.Name == "IsExternalInit");
    Console.WriteLine((p.Name, isInitOnly));
}

// readonly が付いていない record struct でも、プロパティの get メソッドとかには readonly が付く。

static bool isReadOnly(MethodInfo? method)
    => method?.CustomAttributes.Any(a => a.AttributeType.Name == "IsReadOnlyAttribute") == true;

Console.WriteLine(isReadOnly(typeofStruct.GetProperty("X")?.GetGetMethod()));

// PrintMembers、GetHashCode とかにも付くはず(.NET 6 Preview 7 時点ではまだっぽい？)
//Console.WriteLine(isReadOnly(t.GetMethod("PrintMembers", BindingFlags.NonPublic | BindingFlags.Instance)));
//Console.WriteLine(isReadOnly(t.GetMethod("GetHashCode")));

// record struct には EqualityContract と <Clone>$ はない。
Console.WriteLine(typeofStruct.GetMethod("<Clone>$"));
Console.WriteLine(typeofStruct.GetProperty("EqualityContract", BindingFlags.NonPublic | BindingFlags.Instance));

// ReadOnlyPoint も X, Y プロパティを持ってるけど、get; init;
foreach (var p in typeof(ReadOnlyPoint).GetProperties())
{
    var isInitOnly = p.GetSetMethod()?.ReturnParameter.GetRequiredCustomModifiers().Any(x => x.Name == "IsExternalInit");
    Console.WriteLine((p.Name, isInitOnly));
}

// 以下、RecordStruct.csproj と一緒。
record struct Point(int X, int Y);
readonly record struct ReadOnlyPoint(int X, int Y);
record class ClassPoint(int X, int Y);
