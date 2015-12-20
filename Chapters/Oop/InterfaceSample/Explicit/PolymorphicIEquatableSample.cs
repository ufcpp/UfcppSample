namespace InterfaceSample.Explicit
{
    using System;

    abstract class Shape : IEquatable<Shape>
    {
        public abstract bool Equals(Shape other);
    }

    class Rectangle : Shape, IEquatable<Rectangle>
    {
        public double Width { get; set; }
        public double Height { get; set; }

        public override bool Equals(Shape other) => Equals(other as Rectangle);

        public bool Equals(Rectangle other)
            => other != null && Width == other.Width && Height == other.Height;
    }

    class Circle : Shape, IEquatable<Circle>
    {
        public double Radius { get; set; }

        public override bool Equals(Shape other) => Equals(other as Circle);

        public bool Equals(Circle other)
            => other != null && Radius == other.Radius;
    }

    class PolymorphicIEquatableSample
    {
        public static void Main()
        {
            var r1 = new Rectangle { Width = 1, Height = 2 };
            var r2 = new Rectangle { Width = 2, Height = 2 };
            var r3 = new Rectangle { Width = 1, Height = 2 };
            var c1 = new Circle { Radius = 1 };
            var c2 = new Circle { Radius = 2 };
            var c3 = new Circle { Radius = 1 };

            CompareRectangle(r1, r2); // Rectangle False
            CompareRectangle(r1, r3); // Rectangle True
            CompareCircle(c1, c2);    // Circle    False
            CompareCircle(c1, c3);    // Circle    True
            CompareShape(r1, r2);     // Shape     False
            CompareShape(r1, r3);     // Shape     True
            CompareShape(c1, c2);     // Shape     False
            CompareShape(c1, c3);     // Shape     True
            CompareShape(r1, c1);     // Shape     False
        }

        private static void CompareRectangle(IEquatable<Rectangle> r1, Rectangle r2)
        {
            Console.WriteLine("Rectangle " + r1.Equals(r2));
        }

        private static void CompareCircle(IEquatable<Circle> c1, Circle c2)
        {
            Console.WriteLine("Circle    " + c1.Equals(c2));
        }

        private static void CompareShape(IEquatable<Shape> s1, Shape s2)
        {
            Console.WriteLine("Shape     " + s1.Equals(s2));
        }
    }
}
