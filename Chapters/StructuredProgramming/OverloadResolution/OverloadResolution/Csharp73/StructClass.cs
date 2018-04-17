namespace OverloadResolution.Csharp73.StructClass
{
    using System.Collections.Generic;
    using System.Linq;

   static class ClassExtensions
    {
        // クラスの場合は LINQ の FirstOrDefault そのまま。
        public static T FirstOrNull<T>(this IEnumerable<T> source)
            where T : class
            => source.FirstOrDefault();
    }

    static class StructExtensions
    {
        // 構造体の場合は null 許容型に変える必要がある。
        public static T? FirstOrNull<T>(this IEnumerable<T> source)
            where T : struct
            => source.Select(x => (T?)x).FirstOrDefault();
    }

    class Program
    {
        static void Main()
        {
            // ClassExtensions の方のが呼ばれる。
            new[] { "a", "b", "c" }.FirstOrNull();

            // StructExtensions の方のが呼ばれる。
            new[] { 1, 2, 3 }.FirstOrNull();
        }
    }
}
