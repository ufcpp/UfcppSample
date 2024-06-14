
namespace DistinctTest;

public class TestData
{
    public static int[] Data => _data ??= CreateData();
    private static int[]? _data;

    private static int[] CreateData()
    {
        var r = new Random();
        var data = new int[1000];
        foreach (ref var x in data.AsSpan()) x = r.Next(1, 1000);
        return data;
    }
}
