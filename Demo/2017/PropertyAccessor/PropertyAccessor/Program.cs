using BenchmarkDotNet.Running;

namespace PropertyAccessor
{
    /// <summary>
    /// https://github.com/ufcpp/UfcppUtils/tree/master/Source/DynamicUtils がらみの調査。
    ///
    /// プロパティ名を与えてそれの値を読み出すようなものを
    /// <see cref="System.Linq.Expressions"/>動的コード生成で作るとして、
    /// 
    /// - プロパティ名で switch するようなコードも生成
    /// - プロパティごとにコード生成して、それを string → Func/Action な辞書で管理
    ///   - <see cref="System.Collections.Generic.Dictionary{TKey, TValue}"/> 管理
    ///   - <see cref="System.Collections.Generic.SortedDictionary{TKey, TValue}"/> 管理
    ///
    /// 等のパフォーマンスを調査。
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Test();
            BenchmarkRunner.Run<AccessorBenchmark>();
        }

        private static void Test()
        {
            var x = new AccessorBenchmark();
            x.Setup();
            x.ItemSwitch();
            x.ItemCustom();
            x.ItemDictionary();
            x.ItemImmutableDictionary();
            x.ItemSortedDictionary();
            x.ItemSortedList();
            x.PointSwitch();
            x.PointCustom();
            x.PointDictionary();
            x.PointImmutableDictionary();
            x.PointSortedDictionary();
            x.PointSortedList();
        }
    }
}
