namespace SortVisualizer;

public static class Util
{
    public static int[] GetShuffledArray(int length) => GetShuffledArray(Random.Shared, length);

    public static int[] GetShuffledArray(this Random random, int length)
    {
        var array = new int[length];
        for (int i = 0; i < array.Length; i++) array[i] = i + 1;
        Shuffle(random, array);
        return array;
    }

    public static void Shuffle(int[] array) => Shuffle(Random.Shared, array);

    public static void Shuffle(this Random random, int[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            var j = random.Next(i);
            (array[i], array[j]) = (array[j], array[i]);
        }
    }
}
