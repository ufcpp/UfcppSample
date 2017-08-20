struct Point
{
    public double X;
    public double Y;
    public double Z;

    public Point(double x, double y, double z) => (X, Y, Z) = (x, y, z);
    public override string ToString() => $"{X}, {Y}, {Z}";
}

interface IPointCopier
{
    void Copy(Point[] from, Point[] to);
}
