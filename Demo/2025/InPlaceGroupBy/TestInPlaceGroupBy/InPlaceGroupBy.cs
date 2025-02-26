using InPlaceGroupBy;

namespace TestInPlaceGroupBy;

using static Common;

public class InPlaceGroupByTest
{
    [Fact]
    public void EquivalentToLinq()
    {
        var g1 = Data.GroupBy(x => x.key);

        var data = Data.ToArray().AsSpan();
        var g2 = data.GroupBy(Compare);

        Equals(g1, g2);
    }

    private static void Equals(IEnumerable<IGrouping<string, (string key, int value)>> g1, SortedSpanGrouping<(string key, int value)> g2)
    {
        // Same count
        var count = 0;
        {
            var e = g2.GetEnumerator();
            while (e.MoveNext()) count++;
        }
        Assert.Equal(g1.Count(), count);

        var d = g1.ToDictionary(x => x.Key);

        foreach (var g in g2)
        {
            var key = g[0].key;

            // Same keys
            Assert.True(d.TryGetValue(key, out var g1Item));

            // Same values but order is not guaranteed
            Assert.Equal(
                g1Item.Select(x => x.value).Order(),
                g.ToArray().Select(x => x.value).Order());
        }
    }
}
