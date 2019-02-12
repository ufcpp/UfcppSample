namespace Patterns
{
    public class Line
    {
        public Point Start { get; }
        public Point End { get; }
        public Line(Point s, Point e) => (Start, End) = (s, e);
        public void Deconstruct(out Point s, out Point e) => (s, e) = (Start, End);
    }
}
