namespace DistinctTest;

public class HashTest
{
    [Fact]
    public void EqualToLinqDistictOrder()
    {
        var data = TestData.Data;
        var expected = data.Distinct().Order().ToArray();
        var actual = Distinct.Hash.Distinct(data, stackalloc int[data.Length], x => x, x => x == 0);
        MemoryExtensions.Sort(actual);

        Assert.Equal(expected, actual);
    }
}
