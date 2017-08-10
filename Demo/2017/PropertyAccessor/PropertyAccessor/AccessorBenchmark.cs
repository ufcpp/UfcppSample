using BenchmarkDotNet.Attributes;
using PropertyAccessor.SampleData;
using System;
using System.Collections.Generic;

namespace PropertyAccessor
{
    /// <summary>
    /// 実行結果一例:
    ///                    Method |       Mean |     Error |    StdDev |  Gen 0 | Allocated |
    /// ------------------------- |-----------:|----------:|----------:|-------:|----------:|
    ///                ItemSwitch |   476.5 ns |  4.538 ns |  4.245 ns | 0.0639 |     272 B |
    ///                ItemCustom |   916.9 ns |  3.646 ns |  3.232 ns | 0.0629 |     272 B |
    ///            ItemDictionary | 1,092.8 ns | 28.030 ns | 33.368 ns | 0.0629 |     272 B |
    ///   ItemImmutableDictionary | 1,804.7 ns | 19.790 ns | 18.511 ns | 0.0629 |     272 B |
    ///      ItemSortedDictionary | 4,613.6 ns | 13.748 ns | 11.480 ns | 0.0610 |     272 B |
    ///            ItemSortedList | 4,465.0 ns | 24.166 ns | 22.605 ns | 0.0610 |     272 B |
    ///               PointSwitch |   191.1 ns |  1.429 ns |  1.267 ns | 0.0434 |     184 B |
    ///               PointCustom |   278.3 ns |  2.976 ns |  2.784 ns | 0.0434 |     184 B |
    ///           PointDictionary |   349.1 ns |  3.487 ns |  3.262 ns | 0.0434 |     184 B |
    ///  PointImmutableDictionary |   673.5 ns |  2.679 ns |  2.506 ns | 0.0429 |     184 B |
    ///     PointSortedDictionary | 1,078.4 ns | 16.282 ns | 15.230 ns | 0.0420 |     184 B |
    ///           PointSortedList | 1,069.7 ns | 10.616 ns |  9.931 ns | 0.0420 |     184 B |
    ///
    /// switch の速さは、コード生成が複雑化してしまうつらさあってもおつりが来そう。
    /// switch の次、ハッシュテーブルはやっぱ速いなぁ。
    ///
    /// ちなみに、.NET の実装では、
    /// Dictionary → ハッシュテーブル
    /// ImmutableDictionary → ハッシュテーブル
    /// SortedDictionary → 赤黒ツリー
    /// SortedList → ソート済み配列 + 2文探索
    ///
    /// Custom は、「プロパティ数は固定、かつ、たかだか知れた数しかない」って前提で固定長の配列で作ったハッシュテーブル。
    /// </summary>
    [MemoryDiagnoser]
    public class AccessorBenchmark
    {
        Item item;
        Dictionary<string, object> dicItem;

        Point point;
        Dictionary<string, object> dicPoint;

        [GlobalSetup]
        public void Setup()
        {
            item = new Item(123, "abc", 1.23, 0xde, true, false);
            dicItem = new Dictionary<string, object>
            {
                { nameof(Item.Id), item.Id },
                { nameof(Item.Name), item.Name },
                { nameof(Item.Value), item.Value },
                { nameof(Item.Code), item.Code },
                { nameof(Item.TestCaseForLongPropertyName), item.TestCaseForLongPropertyName },
                { nameof(Item.TestCaseForMultiByteCharacterマルチバイト文字), item.TestCaseForMultiByteCharacterマルチバイト文字 },
            };

            point = new Point(10, 20);
            dicPoint = new Dictionary<string, object>
            {
                { nameof(Point.X), point.X },
                { nameof(Point.Y), point.Y },
            };

            // コード生成の時間は別集計したいので、各 benchmark に時間が乗らないように setup 時点で1回アクセスしておく
            var _1 = Accessors.SwitchCodeGenerator<Item>.Get;
            var _2 = Accessors.EachCodeGenerator<Item>.Items;
            var _3 = Accessors.SwitchCodeGenerator<Point>.Get;
            var _4 = Accessors.EachCodeGenerator<Point>.Items;
        }

        [Benchmark] public void ItemSwitch() => Bench(dicItem, item, x => new Accessors.SwitchAccessor<Item>(x), Item.Equals);
        [Benchmark] public void ItemCustom() => Bench(dicItem, item, x => new Accessors.CustomHashTableAccessor<Item>(x), Item.Equals);
        [Benchmark] public void ItemDictionary() => Bench(dicItem, item, x => new Accessors.DictionaryAccessor<Item>(x), Item.Equals);

        [Benchmark] public void PointSwitch() => Bench(dicPoint, point, x => new Accessors.SwitchAccessor<Point>(x), Point.Equals);
        [Benchmark] public void PointCustom() => Bench(dicPoint, point, x => new Accessors.CustomHashTableAccessor<Point>(x), Point.Equals);
        [Benchmark] public void PointDictionary() => Bench(dicPoint, point, x => new Accessors.DictionaryAccessor<Point>(x), Point.Equals);

        // 以下、はっきり遅いことがわかってるのでベンチマーク省略。
        [Benchmark] public void ItemImmutableDictionary() => Bench(dicItem, item, x => new Accessors.ImmutableDictionaryAccessor<Item>(x), Item.Equals);
        [Benchmark] public void ItemSortedDictionary() => Bench(dicItem, item, x => new Accessors.SortedDictionaryAccessor<Item>(x), Item.Equals);
        [Benchmark] public void ItemSortedList() => Bench(dicItem, item, x => new Accessors.SortedListAccessor<Item>(x), Item.Equals);
                    
        [Benchmark] public void PointImmutableDictionary() => Bench(dicPoint, point, x => new Accessors.ImmutableDictionaryAccessor<Point>(x), Point.Equals);
        [Benchmark] public void PointSortedDictionary() => Bench(dicPoint, point, x => new Accessors.SortedDictionaryAccessor<Point>(x), Point.Equals);
        [Benchmark] public void PointSortedList() => Bench(dicPoint, point, x => new Accessors.SortedListAccessor<Point>(x), Point.Equals);

        static void Bench<T, Accessor>(Dictionary<string, object> expectedDic, T expectedObj, Func<T, Accessor> getAccessor, Func<T, T, bool> equals)
            where T : new()
            where Accessor : IAccessor<T>
        {
            var a = getAccessor(expectedObj);

            foreach (var (key, x) in expectedDic)
            {
                var y = a[key];
                if (!Equals(x, y)) throw new InvalidOperationException();
            }

            var b = getAccessor(new T());

            foreach (var (key, x) in expectedDic)
            {
                b[key] = x;
            }

            if (!equals(expectedObj, b.Value)) throw new InvalidOperationException();
        }
    }
}
