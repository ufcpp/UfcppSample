1.M();

Console.WriteLine(FileLocal.R.M().IsMatch("1234"));

// Program.cs 内でだけ有効なクラス。
// A.cs とかの別のファイルに同名のクラスがいても平気。
file static class X
{
    // なので、この拡張メソッド M も、Program.cs 内でだけ参照可能。
    public static void M(this int x) => Console.WriteLine($"{x} in Program.cs");
}
