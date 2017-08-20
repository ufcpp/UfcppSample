namespace ConsoleApp1.SpanBase
{
    using System;

    // Span<T> を受け付ける版
    class Stream
    {
        // 書き込み先は Safe 版と同じ
        byte[] _buffer = new byte[100];
        int _offset = 0;

        public void Read(Span<byte> data)
        {
            var s = new Span<byte>(_buffer, _offset, data.Length);
            s.CopyTo(data);
            _offset += data.Length;
        }

        public void Write(ReadOnlySpan<byte> data)
        {
            var s = new Span<byte>(_buffer, _offset, data.Length);
            data.CopyTo(s);
            _offset += data.Length;
        }

        public void Seek(int offset) => _offset = offset;
    }

    static class PointExtensions
    {
        public static void Save(this Stream s, Point[] points)
        {
            Span<Point> span = points;
            s.Write(span.AsBytes());
        }

        public static void Load(this Stream s, Point[] points)
        {
            Span<Point> span = points;
            s.Read(span.AsBytes());
        }
    }

    struct Copier : IPointCopier
    {
        public void Copy(Point[] from, Point[] to)
        {
            var s = new Stream();
            s.Save(from);
            s.Seek(0);
            s.Load(to);
        }
    }
}
