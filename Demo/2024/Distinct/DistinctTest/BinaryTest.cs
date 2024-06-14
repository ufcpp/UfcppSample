
namespace DistinctTest;

public class BinaryTest
{
    [Fact]
    public void EqualToLinqDistictOrder()
    {
        var data = TestData.Data;
        var expected = data.Distinct().Order().ToArray();
        var actual = Distinct.Binary.Distinct(data, stackalloc int[data.Length]);

        Assert.Equal(expected, actual);
    }
}
