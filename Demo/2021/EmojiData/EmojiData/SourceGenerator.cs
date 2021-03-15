﻿using System.IO;
using System.Linq;
using System.Text;

namespace EmojiData
{
    class SourceGenerator
    {
        public static void WriteLineTest(Rune[][] emojiSequenceList)
        {
            foreach (var seq in emojiSequenceList.Take(100))
            {
                var s = Encoder.ToString(seq);

                foreach (var c in s)
                {
                    System.Console.Write($"{(int)c:X4} ");
                }
                System.Console.WriteLine();
                System.Console.WriteLine(Encoder.ToEscapedString(seq));
            }
        }

        public static void WriteStringArray(Rune[][] emojiSequenceList, string path)
        {
            using var w = new StreamWriter(File.OpenWrite(path), Encoding.UTF8);
            WriteStringArray(emojiSequenceList, w);
        }

        public static void WriteStringArray(Rune[][] emojiSequenceList, TextWriter writer)
        {
            writer.Write(@"        public static readonly string[] RgiEmojiSequenceList = new[]
        {
");

            foreach (var seq in emojiSequenceList)
            {
                var s = Encoder.ToEscapedString(seq);
                writer.Write(@"            """);
                writer.Write(s);
                writer.Write(@""",
");
            }

            writer.Write(@"        };
");
        }
    }
}