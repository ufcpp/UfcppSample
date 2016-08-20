namespace ExtensionMethods.InstanceAndExtension
{
    using static System.Console;

    class X
    {
        public void F(object x) => WriteLine($"object {x}");
        public void F(string x) => WriteLine($"string {x}");
    }

    static class XExtensions
    {
        public static void F(this X @this, int x) => WriteLine($"int {x}");

    }

    class Program
    {
        static void Main(string[] args)
        {
            var x = new X();
            x.F("abc"); // string のが呼ばれる
            x.F(10);    // int な拡張が増えたものの、インスタンス メソッド優先 object のが呼ばれる
        }
    }
}
