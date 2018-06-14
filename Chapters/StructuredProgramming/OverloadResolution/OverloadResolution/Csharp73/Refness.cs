namespace OverloadResolution.Csharp73.Refness
{
    static class Extensions
    {
        // ref の有無の差 + 型制約
        public static void M<T>(this ref T x) where T : struct { }
        public static void M<T>(this T x) where T : class { }
    }

    class Program
    {
        static void Main()
        {
            "abc".M();

            var x = 123;
            x.M();
            // ただ、ref 拡張メソッドの性質上、123.M() とは呼べない(リテラルがダメ)
            // また、DateTime.Now.M() とかもダメ(プロパティ越しがダメ)
        }
    }
}
