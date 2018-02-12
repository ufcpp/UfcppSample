using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// .NET の <see cref="string.Substring(int, int)"/> はなんで常に新しい別の string インスタンスを new するの？
/// インデクサーとか GetEnumerator とかを virtual にして、元の文字列 + 開始インデックス + 文字数 で持つ方がアロケーションが減って速いんじゃないの？
/// とか言う人がごくまれにいるんだけども。
///
/// 実際には、gen0 GC よりも virtual の方が負担高め。
///
/// と言うのを示すために、インデクサーが virtual な string 型を作ったもの。
/// </summary>
abstract class AbstractString : IEnumerable<char>
{
    public abstract char this[int index] { get; }
    public abstract int Length { get; }
    public abstract AbstractString Substring(int start, int count);
    public AbstractString Substring(int start) => Substring(start, Length - start);
    public abstract IEnumerator<char> GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

/// <summary>
/// 通常の文字列。
/// 長さ分の配列を確保してる。
/// </summary>
class FullString : AbstractString
{
    private char[] _buffer;

    public FullString(char[] buffer) => _buffer = buffer;
    public FullString(string s)
    {
        var buffer = new char[s.Length];
        for (int i = 0; i < buffer.Length; i++)
        {
            buffer[i] = s[i];
        }
        _buffer = buffer;
    }

    public override char this[int index] => _buffer[index];
    public override int Length => _buffer.Length;
    public override AbstractString Substring(int start, int count) => new StringSpan(_buffer, start, count);
    public override IEnumerator<char> GetEnumerator() => ((IEnumerable<char>)_buffer).GetEnumerator();
}

/// <summary>
/// Substring 用の文字列。
/// 配列 + 開始インデックス + 長さ。
/// </summary>
class StringSpan : AbstractString
{
    private char[] _buffer;
    private int _start;
    private int _count;

    public StringSpan(char[] buffer, int start, int count)
    {
        if (start + count >= buffer.Length) throw new IndexOutOfRangeException();
        _buffer = buffer;
        _start = start;
        _count = count;
    }

    public override char this[int index] => index < _count ? _buffer[_start + index] : throw new IndexOutOfRangeException();
    public override int Length => _count;
    public override AbstractString Substring(int start, int count) => new StringSpan(_buffer, start + start, count);
    public override IEnumerator<char> GetEnumerator()
    {
        var a = _buffer;
        var end = _start + _count;
        for (int i = _start; i < end; i++)
        {
            yield return a[i];
        }
    }
}
