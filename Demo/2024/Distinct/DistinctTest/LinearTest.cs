
namespace DistinctTest;

public class LinearTest
{
    [Fact]
    public void EqualToLinqDistict()
    {
        var data = TestData.Data;
        var expected = data.Distinct().ToArray();
        var actual = Distinct.Linear.Distinct(data, stackalloc int[data.Length]);

        Assert.Equal(expected, actual);
    }
}
