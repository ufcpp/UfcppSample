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
        bool _crlf;

        public LineReader(Stream stream, Span<byte> initialBuffer)
        {
            _stream = stream;
            _buffer = initialBuffer;
            _previous = 0;
            _read = 0;
            _crlf = false;
        }

        public Span<byte> ReadLine()
        {
            var buffer = _buffer;

            if (buffer.Length == 0) return default;

            var read = _read;
            var prev = _previous;
            var s = _stream;

            // 前回返した分、前に詰める
            if (read != prev)
            {
                buffer.Slice(prev, read - prev).CopyTo(buffer.Slice(0, read - prev));
                read -= prev;
            }
            else read = 0;

            // 読み取りループ
            while (true)
            {
                if (read != 0)
                {
                    var (index, crlf) = IndexOfCrLf(buffer.Slice(0, read));

                    // \r\n の時、前に \r までしか読んでなかったらバグる…
                    // この対処も重たいんだけど、\r\n のためにもっと頑張る気にはなれず
                    if (index == 0 && crlf == 1 && _crlf)
                    {
                        buffer.Slice(1, read - 1).CopyTo(buffer.Slice(0, read - 1));
                        (index, crlf) = IndexOfCrLf(buffer.Slice(0, read));
                    }

                    // 読んである範囲に改行あり
                    if (crlf != 0)
                    {
                        _read = read;
                        _previous = index + crlf;
                        _buffer = buffer;
                        return buffer.Slice(0, index);
                    }
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

        (int index, int crlf) IndexOfCrLf(Span<byte> buffer)
        {
            var i = buffer.IndexOfAny(crlf);
            if (i < 0) return (buffer.Length, 0);

            if(buffer[i] == '\r')
            {
                if (i + 1 < buffer.Length && buffer[i + 1] == '\n')
                {
                    _crlf = true;
                    return (i, 2);
                }
            }
            return (i, 1);
        }
    }
}
