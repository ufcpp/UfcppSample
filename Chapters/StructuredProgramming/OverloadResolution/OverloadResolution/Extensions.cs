namespace OverloadResolution.Extensions
{
    using System;

    class A
    {
        public void M() => Console.WriteLine("instance");
    }

    static class Extensions
    {
        public static void M(this A a) => Console.WriteLine("extension");
    }

    class Program
    {
        static void Main()
        {
            // instance の方が呼ばれる
            new A().M();

            // A 自身が M を持っている以上、↑の書き方で拡張メソッドの方は呼べない
            // 以下のように、普通に静的メソッドとして呼ぶ必要がある
            Extensions.M(new A());
        }
    }
}
