namespace ConsoleApp1.Safe
{
    using System;

    // System.IO.Stream もどき(簡易実装)
    // デモ用に固定長の配列相手に読み書き。
    class Stream
    {
        // 通常は読み書き状況に応じて増減させたり、
        // 場合によってはnative実装でnative(GC管理外)メモリになっている。
        byte[] _buffer = new byte[100];
        int _offset = 0;

        public void Read(byte[] data, int offset, int length)
        {
            Buffer.BlockCopy(_buffer, _offset, data, offset, length);
            _offset += length;
        }

        public void Write(byte[] data, int offset, int length)
        {
            Buffer.BlockCopy(data, offset, _buffer, _offset, length);
            _offset += length;
        }

        public void Seek(int offset) => _offset = offset;
    }

    static class PointExtensions
    {
        public static void Save(this Stream s, Point[] points)
        {
            foreach (var p in points)
            {
                byte[] b = BitConverter.GetBytes(p.X);
                s.Write(b, 0, b.Length);

                b = BitConverter.GetBytes(p.Y);
                s.Write(b, 0, b.Length);

                b = BitConverter.GetBytes(p.Z);
                s.Write(b, 0, b.Length);
            }
        }

        public static void Load(this Stream s, Point[] points)
        {
            var buffer = new byte[8];
            for (int i = 0; i < points.Length; i++)
            {
                s.Read(buffer, 0, 8);
                points[i].X = BitConverter.ToDouble(buffer, 0);

                s.Read(buffer, 0, 8);
                points[i].Y = BitConverter.ToDouble(buffer, 0);

                s.Read(buffer, 0, 8);
                points[i].Z = BitConverter.ToDouble(buffer, 0);
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
