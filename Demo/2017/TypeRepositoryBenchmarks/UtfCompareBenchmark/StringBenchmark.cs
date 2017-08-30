using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;
using System.Linq;
using System.Text;

/// <summary>
/// UTF8 な byte 列と UTF16 な char 列(string)の比較はどうするのは速いのかなぁと。
///
///                  Method |      Mean |     Error |    StdDev |   Gen 0 | Allocated |
/// ----------------------- |----------:|----------:|----------:|--------:|----------:|
///  EncodingGetBytesUnsafe | 105.58 us | 0.4337 us | 0.4057 us |       - |       0 B |
///        EncodingGetBytes | 158.86 us | 1.1790 us | 0.9205 us | 17.5781 |   73832 B |
///       EncodingGetString | 204.73 us | 0.9916 us | 0.8280 us | 17.8223 |   75728 B |
///  EncodingGetCharsUnsafe | 136.66 us | 0.9853 us | 0.9216 us |       - |       0 B |
///        StringToUtf8Byte | 291.99 us | 1.0604 us | 0.8855 us |       - |       0 B |
///              MyGetBytes |  95.09 us | 0.5053 us | 0.4479 us |       - |       0 B |
///
/// 結論としては stackalloc で取った領域に対して書き込んだ結果を、
/// <see cref="SpanExtensions.SequenceEqual(ReadOnlySpan{byte}, ReadOnlySpan{byte})"/> するのが最速。
/// スタックの性質上、あんまり長い文字列に対しては使えないけども。
///
/// その方式なら標準の<see cref="Encoding.GetBytes(char*, int, byte*, int)"/>で十分(<see cref="EncodingGetBytesUnsafe"/>)。
///
/// 一応、範囲チェックとかを微妙にさぼることで、<see cref="Encoding.GetBytes(char*, int, byte*, int)"/>をちょっとだけ抜けることは確認(<see cref="MyGetBytes"/>)。
///
/// corefxlab 実装の System.Text.Primitives の Encoders が速く安定してくれれば…
/// ↑ byte* を int* 扱いして4文字同時エンコード/デコードとかやってるんで、さすがに速そう。
/// </summary>
[MemoryDiagnoser]
public class StringBenchmark
{
    static readonly Encoding utf8 = new UTF8Encoding(false);
    const int N = 1000;
    (string utf16, byte[] utf8)[] items;

    [GlobalSetup]
    public void Setup()
    {
        var r = new Random();

        items = new(string, byte[])[N];

        for (int n = 0; n < items.Length; n++)
        {
            var len = r.Next(0, 5) + r.Next(0, 5) + r.Next(0, 5) + r.Next(0, 5) + r.Next(0, 5) + r.Next(0, 5);
            int[] cp = new int[len];
            for (int i = 0; i < len; i++)
            {
                cp[i] = r.Next(1, 0xFFFFF);
            }

            var s = Utf32ToString(cp);
            var b = utf8.GetBytes(s);

            items[n] = (s, b);
        }
    }

    private static unsafe string Utf32ToString(int[] cp)
    {
        if (cp.Length == 0) return "";

        fixed (int* p = cp)
        {
            return Encoding.UTF32.GetString((byte*)p, cp.Length * 4);
        }
    }

    [Benchmark]
    public void EncodingGetBytesUnsafe()
    {
        foreach (var item in items)
        {
            unsafe
            {
                int len = item.utf16.Length * 3;
                var buf = stackalloc byte[len + 1];
                fixed (char* p = item.utf16)
                {
                    len = utf8.GetBytes(p, item.utf16.Length, buf, len);
                }
                var b = new Span<byte>(buf, len);
                if (!b.SequenceEqual(item.utf8)) throw new Exception();
            }
        }
    }

    [Benchmark]
    public void EncodingGetBytes()
    {
        foreach (var item in items)
        {
            Span<byte> b = utf8.GetBytes(item.utf16);
            if (!b.SequenceEqual(item.utf8)) throw new Exception();
        }
    }

    [Benchmark]
    public void EncodingGetString()
    {
        foreach (var item in items)
        {
            var s = utf8.GetString(item.utf8);
            if (item.utf16 != s) throw new Exception();
        }
    }

    [Benchmark]
    public void EncodingGetCharsUnsafe()
    {
        foreach (var item in items)
        {
            unsafe
            {
                fixed (byte* p = item.utf8)
                {
                    var len = item.utf8.Length;
                    char* c = stackalloc char[len];
                    len = utf8.GetChars(p, len, c, len);

                    fixed (char* p16 = item.utf16)
                    {
                        Span<char> a = new Span<char>(c, len);
                        Span<char> b = new Span<char>(p16, item.utf16.Length);
                        if (!a.SequenceEqual(b)) throw new Exception();
                    }
                }
            }
        }
    }

    [Benchmark]
    public void StringToUtf8Byte()
    {
        foreach (var item in items)
        {
            unsafe
            {
                var buf = stackalloc byte[item.utf16.Length * 3];
                var u = item.utf16.AsBytes();

                var p = buf;
                foreach (var x in item.utf16.AsBytes())
                {
                    *p = x;
                    ++p;
                }
                var len = p - buf;

                var b = new Span<byte>(buf, (int)len);
                if (!b.SequenceEqual(item.utf8)) throw new Exception();
            }
        }
    }

    [Benchmark]
    public void MyGetBytes()
    {
        foreach (var item in items)
        {
            unsafe
            {
                var len = item.utf16.Length * 3;
                var p = stackalloc byte[len];
                Span<byte> buf = new Span<byte>(p, len);
                len = item.utf16.GetBytes(buf);
                buf = buf.Slice(0, len);
                if (!buf.SequenceEqual(item.utf8)) throw new Exception();
            }
        }
    }
}
