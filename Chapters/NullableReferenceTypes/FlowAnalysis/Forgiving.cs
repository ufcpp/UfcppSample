namespace FlowAnalysis.Forgiving
{


    class Program
    {
#nullable enable
        string NotNull = null!;

        static void Main()
        {
            string? s = null;

            var l = s!.Length; // (s!).Length の意味
            var b = s !is null; // (s!) is null の意味
        }
    }
}
