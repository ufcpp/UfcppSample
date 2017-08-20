using ConsoleApp1.Unsafe;

namespace ConsoleApp1.Unsafe
{
    using System;

    // ポインターを受け付ける Unsafe 版
    unsafe class Stream
    {
        // 書き込み先は Safe 版と同じ
        byte[] _buffer = new byte[100];
        int _offset = 0;

        public void Read(byte* data, int length)
        {
            fixed (byte* p = _buffer)
            {
                Buffer.MemoryCopy(p + _offset, data, length, length);
            }
            _offset += length;
        }

        public void Write(byte* data, int length)
        {
            fixed (byte* p = _buffer)
            {
                Buffer.MemoryCopy(data, p + _offset, 100 - _offset, length);
            }
            _offset += length;
        }

        public void Seek(int offset) => _offset = offset;
    }
}

namespace ConsoleApp1.Unsafe1
{
    unsafe static class PointExtensions
    {
        public static void Save(this Stream s, Point[] points)
        {
            var buffer = stackalloc byte[sizeof(double)];
            var pb = (double*)buffer;

            foreach (var p in points)
            {
                *pb = p.X;
                s.Write(buffer, sizeof(double));
                *pb = p.Y;
                s.Write(buffer, sizeof(double));
                *pb = p.Z;
                s.Write(buffer, sizeof(double));
            }
        }

        public static void Load(this Stream s, Point[] points)
        {
            var buffer = stackalloc byte[sizeof(double)];
            var pb = (double*)buffer;

            for (int i = 0; i < points.Length; i++)
            {
                s.Read(buffer, sizeof(double));
                points[i].X = *pb;
                s.Read(buffer, sizeof(double));
                points[i].Y = *pb;
                s.Read(buffer, sizeof(double));
                points[i].Z = *pb;
            }
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

namespace ConsoleApp1.Unsafe2
{
    unsafe static class PointExtensions
    {
        public static void Save(this Stream s, Point[] points)
        {
            fixed(Point* p = points)
            {
                s.Write((byte*)p, 24 * points.Length);
            }
        }

        public static void Load(this Stream s, Point[] points)
        {
            fixed (Point* p = points)
            {
                s.Read((byte*)p, 24 * points.Length);
            }
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
