﻿using RgiSequenceFinder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

using var writer = new StreamWriter("RgiTable.Generated.cs", false, Encoding.UTF8);

writer.Write(@"namespace RgiSequenceFinder
{
    partial class RgiTable
    {
        private static int FindKeycap(Keycap key) => key.Value switch
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

writer.Write(@"            _ => -1,
        };

        private static int FindRegion(RegionalIndicator region) => region.First switch
        {
");

foreach (var g in riFlags.GroupBy(x => x.code.First))
{
    writer.Write("            (byte)'");
    writer.Write((char)g.Key);
    writer.Write(@"' => region.Second switch
            {
");

    foreach (var (ri, index) in g)
    {
        writer.Write("                (byte)'");
        writer.Write((char)ri.Second);
        writer.Write("' => ");
        writer.Write(index);
        writer.Write(@",
");
    }

    writer.Write(@"                _ => -1,
            },
");
}

writer.Write(@"            _ => -1,
        };

        private static int FindTag(TagSequence tags) => tags.LongValue switch
        {
");

foreach (var (tags, index) in tagFlags)
{
    writer.Write("            0x");
    writer.Write(tags.LongValue.ToString("X"));
    writer.Write("UL => ");
    writer.Write(index);
    writer.Write(@",
");
}

// other 分、いったん Dictionary 実装する。
// switch のネストになるようにコード生成したい。
// \uD83D から始まる文字(1F000 台の文字の high surrogate)ばっかりなのでものすごいデータ量減るはずなのと、
// 今の Dictionary 実装だと Substring (新規 string インスタンスのアロケーション)が発生するので避けたい。
// (今のこの Dictionary 実装だと 80 KB くらいバイナリサイズが膨らむ。)

writer.Write(@"            _ => -1,
        };

        private static System.Collections.Generic.Dictionary<string, int> _otherTable = new()
        {
");

foreach (var (s, index) in others)
{
    writer.Write("            { \"");

    foreach (var c in s)
    {
        writer.Write("\\u");
        writer.Write(((int)c).ToString("X4"));
    }

    writer.Write("\", ");
    writer.Write(index);
    writer.Write(@" },
");
}

writer.Write(@"        };

        private static int FindOther(string s) => _otherTable.TryGetValue(s, out var v) ? v : -1;
    }
}
");