namespace SampleProject
{
    internal class A
    {
        public int X { get; }
    }

    public sealed class A1
    {
        public int X { get; set; }
        public int Y { get; init; }
    }

    record A2;
    record A3(int X);
    record struct A4(int X);
    record class A5<T>(T X);
}
