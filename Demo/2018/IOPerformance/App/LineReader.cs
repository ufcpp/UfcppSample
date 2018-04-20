using System;
using System.IO;

namespace App
{
    ref struct LineReader
    {
        readonly Stream _stream;
        Span<byte> _buffer;
        int _previous;
        int _read;

        public LineReader(Stream stream, Span<byte> initialBuffer)
        {
            _stream = stream;
            _buffer = initialBuffer;
            _previous = 0;
            _read = 0;
        }

        public Span<byte> ReadLine()
        {
            var buffer = _buffer;

            if (buffer.Length == 0) return default;

            var read = _read;
            var prev = _previous;
            var s = _stream;

            // 前回返した分、前に詰める
            buffer.Slice(prev, read - prev).CopyTo(buffer.Slice(0, read - prev));
            read -= prev;

            // 読み取りループ
            while (true)
            {
                var (index, crlf) = IndexOfCrLf(buffer.Slice(0, read));

                // 読んである範囲に改行あり
                if (crlf != 0)
                {
                    _read = read;
                    _previous = index + crlf;
                    _buffer = buffer;
                    return buffer.Slice(0, index);
                }

                // 続きを読む
                if (read == buffer.Length)
                {
                    var newBuffer = new byte[buffer.Length * 2];
                    buffer.CopyTo(newBuffer);
                    buffer = newBuffer;
                }

                var next = s.Read(buffer.Slice(read));

                // 末尾
                if(next == 0)
                {
                    _buffer = default;
                    return buffer.Slice(0, read);
                }

                read += next;
            }
        }

        static readonly byte[] crlf = new byte[] { (byte)'\r', (byte)'\n' };

        static (int index, int crlf) IndexOfCrLf(Span<byte> buffer)
        {
            var i = buffer.IndexOfAny(crlf);
            if (i < 0) return (buffer.Length, 0);

            if(buffer[i] == '\r')
            {
                if (i + 1 < buffer.Length && buffer[i + 1] == '\n') return (i, 2);
            }
            return (i, 1);
        }
    }
}
