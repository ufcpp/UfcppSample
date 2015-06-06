namespace NotifyPropertyChangedGeneratorDemo
{
    class Program
    {
        static void Main(string[] args)
        {
        }
    }

    [Notify]
    public class Sample1
    {
        public string Name { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }

    [Notify(NotifyCompareMethod.None)]
    public class Sample2
    {
        public string Name { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}
