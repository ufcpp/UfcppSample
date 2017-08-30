using System;
using System.Collections.Generic;
using System.Collections;

/// <summary>
/// string を UTF8 エンコードした結果を、アロケーションなしで1バイトずつ列挙するための型。
///
/// 没。
/// 遅い。
/// MoveNext() → Current でアクセスするのとかほんと遅い。
/// これだったら<see cref="Encoding.GetString(byte[])"/>/<see cref="Encoding.GetBytes(string)"/>する方が速かった。
///
/// <see cref="IEnumerable{byte}"/>の状態で SequenceEqual すると絶望的に遅くて、
/// 結局それも、stackalloc した領域にコピーしてから<see cref="SpanExtensions.SequenceEqual(Span{byte}, ReadOnlySpan{byte})"/>する方がだいぶ速かった。
/// (やっぱ、<see cref="Span{T}"/>みたいにメモリが連続的に配置されている前提があるとむちゃくちゃ速い。)
/// </summary>
public struct StringToUtf8Byte : IEnumerable<byte>
{
    string _s;

    public StringToUtf8Byte(string s) => _s = s;

    public Enumerator GetEnumerator() => new Enumerator(_s);
    IEnumerator<byte> IEnumerable<byte>.GetEnumerator() => GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public struct Enumerator : IEnumerator<byte>
    {
        string _s;
        int _index;
        byte _c0;
        byte _c1;
        byte _c2;
        byte _c3;
        int _rest;

        public Enumerator(string s) : this()
        {
            _s = s;
        }

        public byte Current
        {
            get
            {
                switch (_rest)
                {
                    default: throw new IndexOutOfRangeException();
                    case 0: return _c0;
                    case 1: return _c1;
                    case 2: return _c2;
                    case 3: return _c3;
                }
            }
        }

        public bool MoveNext()
        {
            --_rest;
            if (_rest < 0)
            {
                if (_index == _s.Length) return false;

                var c = _s[_index];

                if (c < 0x80)
                {
                    _c0 = (byte)c;
                    _rest = 0;
                }
                else if (c < 0b1000_0000_0000)
                {
                    _c0 = (byte)(0b10000000 | c & 0b11_1111);
                    _c1 = (byte)(0b11000000 | (c >> 6) & 0b1_1111);
                    _rest = 1;
                }
                else if (!char.IsHighSurrogate(c))
                {
                    _c0 = (byte)(0b10000000 | c & 0b11_1111);
                    _c1 = (byte)(0b10000000 | (c >> 6) & 0b11_1111);
                    _c2 = (byte)(0b11100000 | (c >> 12) & 0b1111);
                    _rest = 2;
                }
                else
                {
                    var cp = (uint)(c & 0b0011_1111_1111) + 0b0100_0000;

                    ++_index;
                    if (_index == _s.Length) return false;

                    cp = (cp << 10) | ((uint)_s[_index] & 0b0011_1111_1111);

                    _c0 = (byte)(0b10000000 | cp & 0b11_1111);
                    _c1 = (byte)(0b10000000 | (cp >> 6) & 0b11_1111);
                    _c2 = (byte)(0b10000000 | (cp >> 12) & 0b11_1111);
                    _c3 = (byte)(0b11110000 | (cp >> 18) & 0b111);
                    _rest = 3;
                }

                ++_index;
            }
            return true;
        }

        object IEnumerator.Current => Current;
        public void Dispose() { }
        void IEnumerator.Reset() => throw new NotImplementedException();
    }
}

public static class StringToUtf8ByteExtensions
{
    /// <summary>
    /// 同上、没。
    /// <see cref="StringToUtf8Byte"/>
    /// </summary>
    public static StringToUtf8Byte AsBytes(this string s) => new StringToUtf8Byte(s);
}
