using System;
using System.Collections.Generic;
using System.Linq;

namespace AsyncInternal
{
    class Data
    {
        public static IEnumerable<string> Indexes = new[] { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve" };

        public static IEnumerable<string> RandomSelect(IEnumerable<string> list)
        {
            var r = new Random();
            return list.Where(_ => r.NextDouble() < 0.5).ToArray();
        }

        public static string GetContent(string index)
        {
            switch (index)
            {
                case "one": return "子 🐭🐀🐁";
                case "two": return "丑 🐮🐄🐂";
                case "three": return "寅 🐯🐅🐆";
                case "four": return "卯 🐰🐇🐈🐱😹😸😺😻😼😾😿🙀";
                case "five": return "辰 🐉🐊";
                case "six": return "巳 🐍";
                case "seven": return "午 🐴🐎🏇";
                case "eight": return "未 🐑🐏🐐";
                case "nine": return "申 🐵🐒";
                case "ten": return "酉 🐓🐔🐣🐤🐥🐦🐧";
                case "eleven": return "戌 🐶🐕🐩";
                case "twelve": return "亥 🐗🐷🐖🐽";
                default: throw new IndexOutOfRangeException();
            }
        }
    }
}
