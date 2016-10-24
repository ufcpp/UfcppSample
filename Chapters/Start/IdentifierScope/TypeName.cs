using System;

namespace Color
{
    public enum Color
    {
        Green,
        Yellow,
        Red,
    }

    public class Sample
    {
        public global::Color.Color Color { get; set; }

        public void M()
        {
            global::Color.Color Color = global::Color.Color.Red;

            Console.WriteLine(Color);
            Console.WriteLine(this.Color);
        }
    }
}
