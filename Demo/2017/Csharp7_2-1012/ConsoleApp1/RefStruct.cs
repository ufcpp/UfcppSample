//#define InvalidCode

using System;

namespace RefStruct
{
    // Span<T> をフィールドとして持つためには ref 修飾が必要。
    // SpanSafety の方で書いた通り、参照を握る(Span<T> を持つ)と、ref に準ずる制限が生じる。
    ref struct Utf16Array
    {
        Span<char> _chars;

        public Utf16Array(Span<char> chars) => _chars = chars;

        public Enumerator GetEnumerator() => new Enumerator(_chars);

        public ref struct Enumerator
        {
            Span<char> _chars;
            int _index;

            public Enumerator(Span<char> chars)
            {
                _chars = chars;
                _index = -1;
            }

            public bool MoveNext() => ++_index < _chars.Length;
            public uint Current
            {
                get
                {
                    var c = _chars[_index];
                    if (!char.IsHighSurrogate(c))
                    {
                        return c;
                    }
                    else
                    {
                        _index++;
                        if (_index >= _chars.Length) throw new FormatException();

                        var x = (c & 0b00000011_11111111U) + 0b100_0000;
                        x <<= 10;
                        x |= (_chars[_index] & 0b00000011_11111111U);

                        return x;
                    }
                }
            }
        }
    }

#if InvalidCode

    // ref を付けてないのでコンパイル エラーに
    struct Error1
    {
        Span<char> _chars;
    }

    // 他にも、インターフェイスを実装できないとかの制限あり
    ref struct Error2 : System.Collections.Generic.IEnumerable<uint>
    {
        Span<char> _chars;
    }

    // クラスもダメ。クラスの時点で ref も付けれない
    ref class Error3
    {
        Span<char> _chars;
    }

#endif

    class Program
    {
        static void Main()
        {
            var chars = new char[] { (char)0x61, (char)0x3B1, (char)0x2135, (char)0x3042, (char)0xD83D, (char)0xDC08 };

            var str = new Utf16Array(chars);

            var i = 0;
            foreach (var cp in str)
            {
                if (i == 0) Console.WriteLine(cp == 'a');
                if (i == 1) Console.WriteLine(cp == 'α');
                if (i == 2) Console.WriteLine(cp == 'ℵ');
                if (i == 3) Console.WriteLine(cp == 'あ');
                if (i == 4) Console.WriteLine(cp == char.ConvertToUtf32("🐈", 0));

                ++i;
            }
        }
    }
}
