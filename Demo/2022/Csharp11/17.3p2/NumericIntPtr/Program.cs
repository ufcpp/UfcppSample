//## IntPtr に直接算術演算が定義された

IntPtr x = 1;
IntPtr y = 2;

// C# 9 の頃から書けたコード:
var w = 3 * (nint)x + (nint)y << 2;

// この行が .NET 7 以降でないとコンパイル通らないコード:
IntPtr z = 3 * x + y << 2;

Console.WriteLine(z);

// C# 9 で nint/nuint が入って、その当時は
// * nint/nuint は内部的には IntPtr/UIntPtr になってる
// * ただ、(当時)IntPtr/UIntPtr には算術演算がなかったので、nint/nuint のときはコンパイラーが特別扱いして算術演算コードを生成してた
// * IntPtr/UIntPtr なのか nint/nuint なのかを区別するために NativeInteger 属性を付けてた
// だった。
//
// .NET 7 でこの区別が不要になる。
// なので、.NET 7 以降をターゲットにした場合、
// * NativeInteger 属性を出力しない
// * NativeInteger 属性が付いてるかどうかは無視する
// * nint/nuint は完全に IntPtr/UIntPtr の別名になった
// となるらしい。
