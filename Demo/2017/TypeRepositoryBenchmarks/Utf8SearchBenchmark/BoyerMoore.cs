using System;

/// <summary>
/// UTF8 な byte 列を、UTF8 のままで文字列検索するために、Boyer Moore アルゴリズムを自前で実装したもの。
/// https://github.com/likejazz/boyer-moore-string-search/blob/master/boyer-moore.c がベース。
/// </summary>
unsafe static class BoyerMoore
{
    static void MakeDelta1(int* delta1, byte* pattern, int patternLength)
    {
        for (var i = 0; i < 256; i++)
        {
            delta1[i] = patternLength;
        }
        for (var i = 0; i < patternLength - 1; i++)
        {
            delta1[pattern[i]] = patternLength - 1 - i;
        }
    }

    static bool IsPrefix(byte* word, int wordLength, int pos)
    {
        var suffixLen = wordLength - pos;

        for (var i = 0; i < suffixLen; i++)
        {
            if (word[i] != word[pos + i])
            {
                return false;
            }
        }
        return true;
    }

    static int SuffixLength(byte* word, int wordLength, int pos)
    {
        int i;
        for (i = 0; (word[pos - i] == word[wordLength - 1 - i]) && (i < pos); i++) ;
        return i;
    }

    static void MakeDelta2(int* delta2, byte* pattern, int patternLength)
    {
        int lastPrefixIndex = 1;

        for (var p = patternLength; p > 0; p--)
        {
            if (IsPrefix(pattern, patternLength, p)) lastPrefixIndex = p;
            delta2[p] = (patternLength - p) + lastPrefixIndex;
        }

        for (var p = 0; p < patternLength - 1; p++)
        {
            var suffixLength = SuffixLength(pattern, patternLength, p);
            var i = patternLength - 1 - suffixLength;
            if (pattern[p - suffixLength] != pattern[i])
            {
                delta2[i] = patternLength - 1 - p + suffixLength;
            }
        }
    }

    public static unsafe int IndexOf(Span<byte> str, Span<byte> pattern)
    {
        fixed (byte* ps = &str.DangerousGetPinnableReference())
        fixed (byte* pp = &pattern.DangerousGetPinnableReference())
            return IndexOf(ps, str.Length, pp, pattern.Length);
    }

    public static int IndexOf(byte* str, int strLength, byte* pattern, int patternLength)
    {
        int* delta1 = stackalloc int[256];
        int* delta2 = stackalloc int[patternLength];
        MakeDelta1(delta1, pattern, patternLength);
        MakeDelta2(delta2, pattern, patternLength);

        var i = patternLength - 1;
        while (i < strLength)
        {
            var j = patternLength - 1;
            while (j >= 0 && (str[i] == pattern[j]))
            {
                --i;
                --j;
            }
            if (j < 0) return i + 1;

            i += Math.Max(delta1[str[i]], delta2[j]);
        }
        return strLength;
    }
}