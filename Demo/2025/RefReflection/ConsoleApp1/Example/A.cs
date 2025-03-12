using ConsoleApp1.Formatter;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace ConsoleApp1.Example;

public partial struct Line(Point start, Point end)
{
    private Point _start = start;
    public Point Start { get => _start; set => _start = value; }

    private Point _end = end;
    public Point End { get => _end; set => _end = value; }

    public Line() : this(default, default) { }
}

public partial struct Point(int x, int y)
{
    private int _x = x;
    public int X { get => _x; set => _x = value; }

    private int _y = y;
    public int Y { get => _y; set => _y = value; }

    public Point() : this(0, 0) { }
}

partial struct Line
{
    public static readonly IFormatter Formetter = new RecordFormatter(
        typeof(Line),
        [
            new(typeof(Point), x => Unsafe.Unbox<Line>(x).Start, (x, v) => Unsafe.Unbox<Line>(x).Start = (Point)v, x => UnsafeRef.Create(ref x.As<Line>()._start)),
            new(typeof(Point), x => Unsafe.Unbox<Line>(x).End, (x, v) => Unsafe.Unbox<Line>(x).End = (Point)v, x => UnsafeRef.Create(ref x.As<Line>()._end)),
        ]
    );

    public override string ToString() => $"{Start} - {End}";
}

partial struct Point
{
    public static readonly IFormatter Formetter = new RecordFormatter(
        typeof(Point),
        [
            new(typeof(int), x => Unsafe.Unbox<Point>(x).X, (x, v) => Unsafe.Unbox<Point>(x).X = (int)v, x => UnsafeRef.Create(ref x.As<Point>()._x)),
            new(typeof(int), x => Unsafe.Unbox<Point>(x).Y, (x, v) => Unsafe.Unbox<Point>(x).Y = (int)v, x => UnsafeRef.Create(ref x.As<Point>()._y)),
        ]
    );

    public override string ToString() => $"({X}, {Y})";
}
