namespace Csharp6.Csharp5
{
    public class Polygon
    {
        private Point[] _vertexes;

        public int Count
        {
            get
            {
                return _vertexes.Length;
            }
        }
        public Point this[int i]
        {
            get
            {
                return _vertexes[i];
            }
        }

        public Polygon(params Point[] vertexes) { _vertexes = vertexes; }
    }
}
