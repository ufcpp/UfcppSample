namespace TypeName1
{
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
            public Color Color { get; set; }

            public void M()
            {
                Color Color = Color.Red;
            }
        }
    }
}
