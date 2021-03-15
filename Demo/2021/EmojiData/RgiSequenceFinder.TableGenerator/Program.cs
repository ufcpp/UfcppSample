using RgiSequenceFinder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

var data = Data.RgiEmojiSequenceList;

var keycaps = new List<(Keycap key, int index)>();
var riFlags = new List<(RegionalIndicator code, int index)>();
var tagFlags = new List<(TagSequence tag, int index)>();
var others = new List<(string emoji, int index)>();

for (int i = 0; i < data.Length; i++)
{
    var seq = data[i];

    var emoji = GraphemeBreak.GetEmojiSequence(seq);
    var (type, len) = emoji;

    if (len != seq.Length) throw new InvalidOperationException("ないはず");

    switch (type)
    {
        default:
        case EmojiSequenceType.NotEmoji:
            throw new InvalidOperationException("ないはず");
        case EmojiSequenceType.Other:
            others.Add((seq, i));
            break;
        case EmojiSequenceType.Keycap:
            keycaps.Add((emoji.Keycap, i));
            break;
        case EmojiSequenceType.Flag:
            riFlags.Add((emoji.Region, i));
            break;
        case EmojiSequenceType.Tag:
            tagFlags.Add((emoji.Tags, i));
            break;
    }
}

using var writer = new StreamWriter("RgiTable.cs", false, Encoding.UTF8);

writer.Write(@"namespace RgiSequenceFinder
{
    class RgiTable
    {
        private static int FindKeycap(Keycap key) => key switch
        {
");

foreach (var (key, index) in keycaps)
{
    writer.Write("            (byte)'");
    writer.Write((char)key.Value);
    writer.Write("' => ");
    writer.Write(index);
    writer.Write(@",
");
}
//Console.WriteLine(riFlags.Count);
//Console.WriteLine(tagFlags.Count);
//Console.WriteLine(others.Count);

writer.Write(@"            _ => -1,
        };
    }
}
");
