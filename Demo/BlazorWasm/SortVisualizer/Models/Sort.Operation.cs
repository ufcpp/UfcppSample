namespace SortVisualizer;

public partial class Sort
{
    public record struct Operation(Kind Kind, int Index1, int Index2);
}
