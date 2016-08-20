namespace ExtensionMethods.Instance
{
    using static System.Console;

    class X
    {
        public void F(object x) => WriteLine($"object {x}");
        public void F(string x) => WriteLine($"string {x}");
    }

    class Program
    {
        static void Main(string[] args)
        {
            var x = new X();
            x.F("abc"); // string のが呼ばれる
            x.F(10);    // int のオーバーロードがないので object のが呼ばれる
        }
    }
}
