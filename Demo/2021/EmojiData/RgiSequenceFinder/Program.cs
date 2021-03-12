using RgiSequenceFinder;
using System;

var count = 0;
foreach (var s in Data.RgiEmojiSequenceList)
{
    if (GraphemeBreak.IsFlag(s) is { Value: >= 0 } code)
    {
        ++count;
        Console.WriteLine(code);
    }
}
Console.WriteLine(count);

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
