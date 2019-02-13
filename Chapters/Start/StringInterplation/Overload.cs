namespace StringInterplation.Overload
{
    using System;

    class Program
    {
        static void Main()
        {
            var (x, y) = (1, 2);

            // string と FormattableString のオーバーロードがあると、string が優先されてしまう。
            M1($"({x}, {y})");
            // 一応、明示的にキャストを書けば呼び分け可能
            M1((FormattableString)$"({x}, {y})");

            // FormattableString 側が呼ばれる
            M2($"({x}, {y})");
            // RawString 側が呼ばれる
            M2("abc");
        }

        // string が優先されるので、M1($"") という書き方では呼び分けできない。
        static void M1(string s) => Console.WriteLine("string: " + s);
        static void M1(FormattableString s) => Console.WriteLine($"format: {s.Format}, args: {string.Join(", ", s.GetArguments())}");

        // M2("") と M2($"") で呼び分けできる。
        static void M2(RawString s) => M1(s.Value);
        static void M2(FormattableString s) => M1(s);

        // オーバーロード解決の優先度をごまかすために、string からの暗黙的型変換を持つ構造体を用意。
        public readonly struct RawString
        {
            public readonly string Value;
            public RawString(string value) => Value = value;
            public static implicit operator RawString(string s) => new RawString(s);

            // これがないとダメみたい
            public static implicit operator RawString(FormattableString s) => throw new InvalidCastException();
        }

        static void M1()
        {
            // string の方が呼ばれる
            M1("");

            // これでも、結局 string の方が呼ばれる
            M1($"");

            // FormattableString の方を呼びたければ明示的なキャストが必要
            M1((FormattableString)$"");
        }

        static void M2()
        {
            // RawString (string) の方が呼ばれる
            M2("");

            // これなら FormattableString の方が呼ばれる
            M2($"");

            // ただ、 + とかを加えてしまうと string 扱いになってしまうので注意
            M2($"" + $"");
        }
    }
}
