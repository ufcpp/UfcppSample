using RgiSequenceFinder;
using System;

var count = 0;
foreach (var s in Data.RgiEmojiSequenceList)
{
    if (GraphemeBreak.IsTagSequence(s) is var len && len > 0)
    {
        ++count;

        var span = s.AsSpan(2);
        while (span.Length >= 2)
        {
            // tag 文字 → 対応する ASCII に変換。
            // high surrogate 無視。
            Console.Write((char)(span[1] - 0xDC00));
            span = span.Slice(2);
        }
        Console.WriteLine();
    }
}
Console.WriteLine(count);

#if false
var count = 0;
foreach (var s in Data.RgiEmojiSequenceList)
{
    if (GraphemeBreak.IsFlagSequence(s) is { Value: >= 0 } code)
    {
        ++count;
        Console.WriteLine(code);
    }
}
Console.WriteLine(count);
#endif
#if false
var count = 0;
foreach (var s in Data.RgiEmojiSequenceList)
{
    if (GraphemeBreak.IsKeycap(s))
    {
        ++count;
        Console.WriteLine(s);
    }
}
Console.WriteLine(count);
#endif
