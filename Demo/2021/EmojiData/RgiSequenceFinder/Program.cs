using RgiSequenceFinder;
using System;

var count = 0;
foreach (var s in Data.RgiEmojiSequenceList)
{
    if (GraphemeBreak.IsFlag(s) > 0)
    {
        ++count;
        Console.WriteLine(s);
    }
}

Console.WriteLine(count);
