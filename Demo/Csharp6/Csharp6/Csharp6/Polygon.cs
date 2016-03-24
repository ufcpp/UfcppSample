namespace Csharp6.Csharp6
{
    public class Polygon
    {
        private Point[] _vertexes;

        public int Count => _vertexes.Length;
        public Point this[int i] => _vertexes[i];

        public Polygon(params Point[] vertexes) { _vertexes = vertexes; }
    }
}
